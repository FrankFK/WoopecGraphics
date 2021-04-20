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

    public class ScreenWriterMockup : IScreenObjectWriter
    {
        private string _animationSequence;

        public int CreateLine()
        {
            return 1;
        }

        public void DrawLine(ScreenLine line)
        {
        }

        public string GetAnimationSequence()
        {
            return _animationSequence;
        }

        public void StartAnimaton(ScreenObject screenObject, System.Action<int> whenFinished)
        {
            _animationSequence += $"{screenObject.ID}-started,";
            foreach (var animation in screenObject.Animations)
            {
                var thread = new Thread(
                    new ThreadStart(() =>
                        {
                            Thread.Sleep(animation.Milliseconds);
                            _animationSequence += $"{screenObject.ID}-finished,";
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
        private int _lastFinishedId = -1;

        [TestMethod]
        public void TwoConsecutiveAnimations_X()
        {
            _actualWriter = new ScreenWriterMockup();
            var objectBroker = new ScreenObjectBroker(_actualWriter, capacity: 10);
            _actualConsumer = objectBroker.Consumer;

            _actualProducer = new ScreenObjectProducer(objectBroker.ObjectChannel);
            var producerThread = new Thread(new ThreadStart(TwoConsecutiveAnimations));
            producerThread.Start();

            var task = _actualConsumer.GetNextObjectForWriterAsync();

            task.ContinueWith((t) =>
            {
                _actualConsumer.SendNextObjectToWriter(t.Result, WhenWriterIsFinished);
            });

            // At the moment idle waiting
            while (true)
            {
                if (_lastFinishedId == 1)
                {
                    var animationSequence = _actualWriter.GetAnimationSequence();

                    animationSequence.Should().Be("0-started,0-finished,1-started,1-finished,");
                    return;
                }
                Thread.Sleep(100);
            }

        }

        private void WhenWriterIsFinished(int id)
        {
            Console.WriteLine($"{id} is finished");
            _lastFinishedId = id;

            var task = _actualConsumer.GetNextObjectForWriterAsync();

            task.ContinueWith((t) =>
            {
                _actualConsumer.SendNextObjectToWriter(t.Result, WhenWriterIsFinished);
            });

        }

        private void TwoConsecutiveAnimations()
        {
            var chainId = 1;
            var firstLineId = _actualProducer.CreateLine();
            var secondLineId = _actualProducer.CreateLine();
            var duration = 1000;

            var firstLine = new ScreenLine() { ID = firstLineId, Point1 = new Vec2D(1, 1), Point2 = new Vec2D(2, 2) };
            firstLine.AddAnimation(
                new ScreenAnimationMovement()
                {
                    ChainID = chainId,
                    Milliseconds = duration,
                    AnimatedProperty = ScreenAnimationMovementProperty.Point2,
                    StartValue = firstLine.Point1
                }
            );

            var secondLine = new ScreenLine() { ID = secondLineId, Point1 = new Vec2D(2, 2), Point2 = new Vec2D(2, 3) };
            secondLine.AddAnimation(
                new ScreenAnimationMovement()
                {
                    ChainID = chainId,
                    Milliseconds = duration,
                    AnimatedProperty = ScreenAnimationMovementProperty.Point2,
                    StartValue = secondLine.Point1,
                    StartWhenPredecessorHasFinished = true, // <-- This is the important part
                }
            );

            _actualProducer.DrawLine(firstLine);
            _actualProducer.DrawLine(secondLine);
        }
    }
}
