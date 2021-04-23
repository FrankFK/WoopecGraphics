using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace TurtleCore.UnitTests
{
    internal record AnimationProtocolEntry(int ID, bool Finished);

    /// <summary>
    /// Mockup for the test
    /// </summary>
    internal class ScreenWriterMockup : IScreenObjectWriter
    {
        private readonly List<AnimationProtocolEntry> _animationProtocol = new();

        public int CreateLine()
        {
            return 1;
        }

        public void DrawLine(ScreenLine line)
        {
        }

        public string GetAnimationSequence()
        {
            var sb = new StringBuilder();
            foreach (var entry in _animationProtocol)
            {
                if (entry.Finished)
                {
                    sb.Append('<');
                    sb.Append(entry.ID);
                    sb.Append(']');
                }
                else
                {
                    sb.Append('[');
                    sb.Append(entry.ID);
                    sb.Append('>');
                }
            }
            return sb.ToString();
        }

        public void StartAnimaton(ScreenObject screenObject, System.Action<int> whenFinished)
        {
            // protocol the start of the animation
            _animationProtocol.Add(new AnimationProtocolEntry(screenObject.ID, false));

            foreach (var animation in screenObject.Animations)
            {
                var thread = new Thread(
                    new ThreadStart(() =>
                        {
                            // simulate the animation
                            Thread.Sleep(animation.Milliseconds);

                            // animation is finished. protocol it
                            _animationProtocol.Add(new AnimationProtocolEntry(screenObject.ID, true));
                            whenFinished(screenObject.ID);
                        }
                    )
                );
                thread.Start();
            }
        }
    }

    /// <summary>
    /// Test the handling of animations.
    /// These tests are more like integration tests than unit tests
    /// </summary>
    [TestClass]
    public class AnimationHandlingTest
    {
        private IScreenObjectProducer _actualProducer;
        private IScreenObjectConsumer _actualConsumer;
        private ScreenWriterMockup _actualWriter;
        private int _stopWhenObjectIsFinished = -1;
        private bool _finished = false;

        [TestMethod]
        public void Case01_TwoConsecutiveAnimations()
        {
            // second animation should start if the first animation is finishedn
            var result = TestSequence(brokerCapacity: 10, animationSequence: "1:1>1000 1:2|1000", stopWhenObjectIsFinished: 2);
            result.Should().Be("[1><1][2><2]");
        }

        [TestMethod]
        public void Case02_TwoParallelAnimations()
        {
            // second animation should start immediately after the first animation is started
            var result = TestSequence(brokerCapacity: 10, animationSequence: "1:1>1000 1:2>1500", stopWhenObjectIsFinished: 2);
            result.Should().Be("[1>[2><1]<2]");
        }

        [TestMethod]
        public void Case03_TwoParallelAnimations()
        {
            // second animation should start immediately after the first animation is started
            // The second animation has a shorter duration than the first animation. Therefore the second should finish before the first is finished
            var result = TestSequence(brokerCapacity: 10, animationSequence: "1:1>1000 1:2>500", stopWhenObjectIsFinished: 1);
            result.Should().Be("[1>[2><2]<1]");
        }

        /// <summary>
        /// Create producer and consumer. Produce a few animations and return the result, that the consumer would produce on the screen
        /// </summary>
        /// <param name="brokerCapacity">Maximal number ob objects in the channel between producer and consumer</param>
        /// <param name="animationSequence">This string describes the animations, that schould be sent to the conusmer</param>
        /// <param name="stopWhenObjectIsFinished">the consumer waits until the animation of the object with this id is finished</param>
        /// <returns>A string that describes the result of the consumer</returns>
        private string TestSequence(int brokerCapacity, string animationSequence, int stopWhenObjectIsFinished)
        {
            _finished = false;
            _stopWhenObjectIsFinished = stopWhenObjectIsFinished;

            // generate a mockup-object, that simulation the draw-operations on the screen (in reality this would be a wpf-canvas)
            _actualWriter = new ScreenWriterMockup();

            // the broker transports screen objects from the producer(s) to the consumer. The consumer sends them to the writer.
            var objectBroker = new ScreenObjectBroker(_actualWriter, brokerCapacity);
            // the one and only consumer
            _actualConsumer = objectBroker.Consumer;

            // It is possible to have multiple producers. In this test we only have one
            // This producer runs in a another thread.
            _actualProducer = new ScreenObjectProducer(objectBroker.ObjectChannel);
            var producerThread = new Thread(
                        new ThreadStart(() =>
                        {
                            AddAnimatedSequence(animationSequence);
                        }
                    )
                );
            producerThread.Start();

            // The consumer runs in this thread. It waits asynchronically for the next object in the channel
            // and sends it to the writer
            var task = _actualConsumer.GetNextObjectForWriterAsync();
            task.ContinueWith((t) =>
            {
                // When the animation of the object is finshed, the method 'WhenWriterIsFinished' is called.
                _actualConsumer.SendNextObjectToWriter(t.Result, WhenWriterIsFinished);
            });

            // The consumer waits until the last object is finished by the writer.
            while (!_finished)
            {
                Thread.Sleep(100);
            }

            // The writer gives us a string that describes the order in which he animated the objects.
            var result = _actualWriter.GetAnimationSequence();

            return result;

        }

        private void WhenWriterIsFinished(int id)
        {
            Console.WriteLine($"{id} is finished");
            if (id == _stopWhenObjectIsFinished || _finished)
            {
                _finished = true;
            }
            else
            {
                // The consumer runs in this thread. It waits asynchronically for the next object in the channel
                // and sends it to the writer
                var task = _actualConsumer.GetNextObjectForWriterAsync();
                task.ContinueWith((t) =>
                {
                    // When the animation of the object is finshed, the method 'WhenWriterIsFinished' is called.
                    _actualConsumer.SendNextObjectToWriter(t.Result, WhenWriterIsFinished);
                });
            }

        }


        /// <summary>
        /// Translate the given string into animated screen objects and send them -- via the producer -- to the channel
        /// </summary>
        /// <param name="animationAsString"></param>
        /// <example>
        ///    Syntax
        ///         animationAsString    ::= animation animation ...
        ///         animation            ::= chainId:objectId>duration       An animation that should start immediately and sould run for duration milliseconds
        ///         animation            ::= chainId:objectId|duration       An animation that should wait for finishing of the previous animation in the same chain 
        ///    Examples
        ///         "1:1>1000 1:2|1000"
        /// </example>
        private void AddAnimatedSequence(string animationAsString)
        {
            var animations = animationAsString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var animation in animations)
            {
                var trimmed = animation.Trim();
                var indexOfChainSep = trimmed.IndexOf(':');
                var chainId = int.Parse(trimmed[..indexOfChainSep]);

                var part2 = trimmed[(indexOfChainSep + 1)..];
                var indexOfGreaterSep = part2.IndexOf('>');
                var indexOfLineSep = part2.IndexOf('|');
                var indexOfSecondSep = (indexOfLineSep >= 0) ? indexOfLineSep : indexOfGreaterSep;

                var objectId = int.Parse(part2[..indexOfSecondSep]);
                var duration = int.Parse(part2[(indexOfSecondSep + 1)..]);

                var startWhenPredecessorHasFinished = (indexOfLineSep >= 0);

                AddAnimatedObject(chainId, objectId, duration, startWhenPredecessorHasFinished);
            }
        }


        private void AddAnimatedObject(int chainId, int objectId, int duration, bool startWhenPredecessorHasFinished)
        {
            var line = new ScreenLine()
            {
                ID = objectId,
            };

            line.AddAnimation(
                new ScreenAnimationMovement()
                {
                    ChainID = chainId,
                    Milliseconds = duration,
                    StartWhenPredecessorHasFinished = startWhenPredecessorHasFinished, 
                }
            );

            _actualProducer.DrawLine(line);
        }

    }
}
