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
            // second animation should start if the first animation is finished
            // example: a turtle (1) makes move (1) 1000 milliseconds (1000). When turtle the move of this turtle is finished (?1) the same turtle makes a second move (2) 1000 milliseconds
            var result = TestSequence(brokerCapacity: 10, animationSequence: "1,1:1000 ?1,2:1000", stopWhenObjectIsFinished: 2);
            result.Should().Be("[1><1][2><2]");
        }

        [TestMethod]
        public void Case02_TwoParallelAnimations()
        {
            // second animation should start immediately after the first animation is started
            // example: a turtle (1) makes move (1) 1000 milliseconds (1000). The same turtle at the same time (1) starts a second move (2) 1500 milliseconds
            var result = TestSequence(brokerCapacity: 10, animationSequence: "1,1:1000 1,2:1500", stopWhenObjectIsFinished: 2);
            result.Should().Be("[1>[2><1]<2]");
        }

        [TestMethod]
        public void Case03_TwoParallelAnimations()
        {
            // second animation should start immediately after the first animation is started
            // The second animation has a shorter duration than the first animation. Therefore the second should finish before the first is finished
            // Example:
            //     We have two turtles (1 and 2).
            //     Turtle 1 makes two animations, where the second should start when the first is finisehd (1,1:1000 ?1,2:500).
            //     Turtle 2 starts its animation, that should be parallel to the animations of turtel 1 (2,3:1200)
            var result = TestSequence(brokerCapacity: 10, animationSequence: "1,1:1000 ?1,2:500 2,3:1200", stopWhenObjectIsFinished: 2);
            result.Should().Be("[1>[3><1][2><3]<2]");

            // Remark: 
            //    The programmer would except the above behaviour if she programs this example:
            //          var turtles = new List<Turtle>;
            //          turtles.Add(new Turtle()); 
            //          turtles.Add(new Turtle()); 
            //          for (int i = 0; i <= 1; i++) {
            //              turtles[i].Left(i*90);
            //              turtles[i].Forward(100);
            //          }
            //
            //    But in many cases this is not the expected behaviour!
            //    Look at this example
            //          var turtle1 = new Turtle()
            //          turtle1.Forward(100);
            //          var turtle2 = new Turtle();
            //          turtle2.Left(90);
            //          turtle2.Forward)50);
            //    In this example the programmer excpects that turtle2 starts moving whne turtle1 has finished its move.
            //    See Case04_SequentialAnimations for this case.
        }

        [TestMethod]
        public void Case04_SequentialAnimations()
        {
            // second animation should start after the first animation is finished
            // The second animation has a shorter duration than the first animation. Therefore the second should finish before the first is finished
            // Example:
            //     We have two turtles (1 and 2).
            //     Turtle 1 makes two animations, where the second should start when the first is finisehd (1,1:1000 ?1,2:500).
            //     Turtle 2 waits for all previous animations of turtle 1 (?(1)) and then the aninmation of turtle 2 is drawn (2,3:1200)
            //             It is important that the next animation of turtle 2 (2:4:500) is not drawn before the first animation of turtle2
            var result = TestSequence(brokerCapacity: 10, animationSequence: "1,1:1000 ?1,2:500 ?(1)2,3:1200 ?2,4:500", stopWhenObjectIsFinished: 4);
            result.Should().Be("[1><1][2><2][3><3][4><4]");
        }

        [TestMethod]
        public void NoEndlessLoopIfProducerThreadHasAnException()
        {
            Action act = () => TestSequence(brokerCapacity: 10, animationSequence: "wrong syntax", stopWhenObjectIsFinished: 2);

            act.Should().Throw<Exception>()
                .WithMessage("Timed out");
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
            _actualWriter.OnAnimationIsFinished += WhenWriterIsFinished;

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
            NextTask();
            /*
            var task = _actualConsumer.GetNextObjectForWriterAsync();
            task.ContinueWith((t) =>
            {
                // When the animation of the object is finshed, the method 'WhenWriterIsFinished' is called.
                _actualConsumer.SendNextObjectToWriter(t.Result, WhenWriterIsFinished);
                NextTask();
            });
            */

            // The consumer waits until the last object is finished by the writer.
            var maxRounds = 100;
            while (!_finished && maxRounds > 0)
            {
                maxRounds--;
                Thread.Sleep(100);
            }

            if (!_finished)
            {
                throw new Exception("Timed out");
            }

            // The writer gives us a string that describes the order in which he animated the objects.
            var result = _actualWriter.GetAnimationSequence();

            return result;

        }

        private void NextTask()
        {
            if (!_finished)
            {
                var task = _actualConsumer.GetNextObjectForWriterAsync();
                task.ContinueWith((t) =>
                {
                    // When the animation of the object is finshed, the method 'WhenWriterIsFinished' is called.
                    _actualConsumer.SendNextObjectToWriter(t.Result);
                    NextTask();
                });
            }
        }

        private void WhenWriterIsFinished(int groupId, int objectId)
        {
            Console.WriteLine($"{objectId} is finished");
            if (objectId == _stopWhenObjectIsFinished || _finished)
            {
                _finished = true;
            }
        }


        /// <summary>
        /// Translate the given string into animated screen objects and send them -- via the producer -- to the channel
        /// </summary>
        /// <param name="animationAsString"></param>
        /// <example>
        ///    Syntax
        ///         animationAsString    ::= animation animation ...
        ///         animation            ::=  groupId,objectId:duration                     An animation that should start immediately and sould run for duration milliseconds
        ///         animation            ::= ?groupId,objectId:duration                     An animation that should wait for finishing of the previous animation in the same animation group
        ///         animation            ::= ?(othergroupid)groupId,objectId:duration       An animation that should wait for finishing of all previous animation in the animation group with othergroupid
        ///    Examples
        ///         "1,1:1000 ?1,2:500 2,3:1200"
        /// </example>
        private void AddAnimatedSequence(string animationAsString)
        {
            try
            {
                var animations = animationAsString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var animation in animations)
                {
                    var toParse = animation.Trim();

                    var startWhenPredecessorHasFinished = false;
                    var otherGroupId = 0;
                    if (toParse.StartsWith('?'))
                    {
                        startWhenPredecessorHasFinished = true;
                        toParse = toParse[1..];
                        if (toParse.StartsWith('('))
                        {
                            var indexOfClosingPara = toParse.IndexOf(')');
                            otherGroupId = int.Parse(toParse[1..indexOfClosingPara]);
                            toParse = toParse[(indexOfClosingPara + 1)..];
                        }
                    }
                    var indexOfComma = toParse.IndexOf(',');
                    var groupId = int.Parse(toParse[..indexOfComma]);

                    toParse = toParse[(indexOfComma + 1)..];
                    var indexOfDoubleColon = toParse.IndexOf(':');
                    var objectId = int.Parse(toParse[..indexOfDoubleColon]);
                    var duration = int.Parse(toParse[(indexOfDoubleColon + 1)..]);

                    AddAnimatedObject(groupId, objectId, duration, startWhenPredecessorHasFinished, otherGroupId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in AddAnimatedSequence");
                Console.WriteLine("\nMessage ---\n{0}", ex.Message);
                Console.WriteLine("\nHelpLink ---\n{0}", ex.HelpLink);
                Console.WriteLine("\nSource ---\n{0}", ex.Source);
                Console.WriteLine("\nStackTrace ---\n{0}", ex.StackTrace);
                Console.WriteLine("\nTargetSite ---\n{0}", ex.TargetSite);
            }
        }


        private void AddAnimatedObject(int groupId, int objectId, int duration, bool startWhenPredecessorHasFinished, int otherGroupId)
        {
            var line = new ScreenLine()
            {
                ID = objectId,
            };

            line.Animation = new ScreenAnimation(groupId);

            if (startWhenPredecessorHasFinished)
            {
                if (otherGroupId != 0)
                    line.Animation.WaitForAnimationsOfGroupID = otherGroupId;
                else
                    line.Animation.WaitForAnimationsOfGroupID = groupId;
            }

            line.Animation.Effects.Add(new ScreenAnimationEffect() { Milliseconds = duration });

            _actualProducer.DrawLine(line);
        }

    }


    internal record AnimationProtocolEntry(int ID, bool Finished);

    /// <summary>
    /// Mockup for the test
    /// </summary>
    internal class ScreenWriterMockup : IScreenObjectWriter
    {
        private readonly List<AnimationProtocolEntry> _animationProtocol = new();

        public void StartAnimaton(ScreenObject screenObject)
        {
            // protocol the start of the animation
            _animationProtocol.Add(new AnimationProtocolEntry(screenObject.ID, false));

            var animation = screenObject.Animation;
            var thread = new Thread(
                new ThreadStart(() =>
                {
                    // simulate the animation
                    Thread.Sleep(animation.Milliseconds);

                    // animation is finished. protocol it
                    _animationProtocol.Add(new AnimationProtocolEntry(screenObject.ID, true));

                    // Inform everyone who wants to know that the animation is finished
                    OnAnimationIsFinished(animation.GroupID, screenObject.ID);
                }
                )
            );
            thread.Start();
        }

        /// <summary>
        /// The Writer calls these events for every animation which is finished
        /// </summary>
        public event AnimationIsFinished OnAnimationIsFinished;

        public void Draw(ScreenObject screenObject)
        {
            // Nothing to do in this test
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

    }

}
