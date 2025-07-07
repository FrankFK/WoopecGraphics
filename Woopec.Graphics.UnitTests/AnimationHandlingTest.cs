using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;
using Woopec.Graphics.InternalCommunication;
using Woopec.Graphics.CommunicatedObjects;
using Woopec.Graphics.InternalBackend;
using Woopec.Graphics.InternalFrontend;

namespace Woopec.Graphics.UnitTests
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
        public void Case05_TwoAnimationsRunInParallelIfFirstAnimationIsFinished()
        {
            // We have two animation (groupId 2 and 3) that should wait until the first animation is finished
            var result = TestSequence(brokerCapacity: 10, animationSequence: "1,1:1000 ?1,2:500 ?(1)2,3:1200 ?2,4:500 ?(1)3,5:1000 ?3,6:1000", stopWhenObjectIsFinished: 6);
            result.Should().Be("[1><1][2><2][3>[5><5][6><3][4><4]<6]");
        }

        [TestMethod]
        public void Case06_AnimationIsFinishedAndAWaitingObjectIsTaken()
        {
            // object 2 waits for object 1, therefore object 3 has to wait also (because the order of the objects of the same group should not change).
            // When object 1 is finished, object 2 and 3 can start. 3 doest not have to wait for 2.
            var result = TestSequence(brokerCapacity: 10, animationSequence: "1,1:500 ?1,2:1000 1,3:500", stopWhenObjectIsFinished: 2);
            result.Should().Be("[1><1][2>[3><3]<2]");
        }

        [TestMethod]
        public void Case07_FirstReadyToRunObjectHasNoAnimation()
        {
            // Groups 2 and 3 are waiting for group 1 to be finished
            // When group 1 is finished, the first objects of groups 2 and 3 can be sent to the writer
            // These objects have no animation! 
            // Therefore it is important that also the next objects of this group are sent to the writer too.
            var result = TestSequence(brokerCapacity: 10, animationSequence: "1,1:100 ?(1)2,2:0 ?(1)3,3:0 ?2,4:100 ?3,5:200", stopWhenObjectIsFinished: 5);
            result.Should().Be("[1><1][2><2][3><3][4>[5><4]<5]");
        }

        [TestMethod]
        public void Case08_ParallelAnimationsOfSameGroupDoNotStartTooMuch()
        {
            var result = TestSequence(brokerCapacity: 10, animationSequence: "1,0:0 ?1,1:100 1,2:50 ?1,3:100 1,4:50 ?1,5:100 1,6:50", stopWhenObjectIsFinished: 5);
            result.Should().Be("[0><0][1>[2><2]<1][3>[4><4]<3][5>[6><6]<5]");
        }

        [TestMethod]
        public void Case09_SecondGroupWaitsForFirstGroup()
        {
            var result = TestSequence(brokerCapacity: 10, animationSequence: "1,11:500 1,12:300 ?(1)2,21:500 2,22:300", stopWhenObjectIsFinished: 21);
            result.Should().Be("[11>[12><12]<11][21>[22><22]<21]");
        }

        [TestMethod]
        public void Case10_WaitForCompletedMovementOf_WorksAsExpected()
        {
            // Test-Sequence is similar to the code on the right.
            // But I changed the sequence a little bit:
            //      To make the results easier to interpret: incremented the ids of the object, such that the moves can be differentiated by the object number
            //      To make the test more deterministic: different durations for the animation, such that the there is a clear order the finishing of parallely started animations
            var testSequence = "";
            testSequence += "1,0:0 ";           // var turtle1 = Turtle.Seymour();
            testSequence += "?1,0:0 ";
            testSequence += "?1,0:0 ";
            testSequence += "?1,0:0 ";
            testSequence += "2,1:0 ";           // var turtle2 = Turtle.Seymour();
            testSequence += "?2,1:0 ";
            testSequence += "?2,1:0 ";
            testSequence += "?2,1:0 ";
            testSequence += "?1,0:575 ";        // turtle1.Left(90);
            testSequence += "?1,0:600 ";        // turtle1.Forward(100); pen    movement
            testSequence += "1,3:700 ";         //                       figure movement
            testSequence += "?2,4:500 ";        // turtle2.Right(90);
                                                // turtle2.WaitForCompletedMovementOf(turtle1);
            testSequence += "??(1)2,5:300 ";    // turtle2.Forward(100); pen    movement
            testSequence += "2,6:250 ";         // turtle2.Forward(100); figure movement
                                                // turtle1.WaitForCompletedMovementOf(turtle2); turtle1.Left(30);
            testSequence += "??(2)1,7:100 ";    // turtle1.Left(30);     figure movement (figure waits for completed movement)
            testSequence += "?1,8:400 ";        // turtle1.Forward(100); pen    movement 
            testSequence += "1,9:300 ";         // turtle1.Forward(100); figure movement

            testSequence += "??(1)10,10:1500";   // New group 10 as a sentinel at the end (waits for the last group (9) of the real testsequence). The test finished when this group is finished

            var result = TestSequence(brokerCapacity: 100, animationSequence: testSequence, stopWhenObjectIsFinished: 10);
            result.Should().Be("[0><0][0><0][0><0][0><0][1><1][1><1][1><1][1><1][0>[4><4]<0][0>[3><0]<3][5>[6><6]<5][7><7][8>[9><9]<8][10><10]");
        }

        [TestMethod]
        public void NoEndlessLoopIfProducerThreadHasAnException()
        {
            Action act = () => TestSequence(brokerCapacity: 10, animationSequence: "wrong syntax", stopWhenObjectIsFinished: 2);

            act.Should().Throw<Exception>()
                .WithMessage("Timed out*");
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

            // generate a mockup-object, that simulates the draw-operations on the screen (in reality this would be a wpf-canvas)
            _actualWriter = new ScreenWriterMockup();
            _actualWriter.OnAnimationIsFinished += WhenWriterIsFinished;

            // the broker transports screen objects from the producer(s) to the consumer. The consumer sends them to the writer.
            var objectBroker = new CommunicationBroker(_actualWriter, brokerCapacity);
            // the one and only consumer
            _actualConsumer = objectBroker.ScreenObjectConsumer;

            // It is possible to have multiple producers. In this test we only have one
            // This producer runs in a another thread.
            _actualProducer = new ScreenObjectProducer(objectBroker.ScreenObjectChannelForWriter);
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

            // The consumer waits until the last object is finished by the writer.
            var maxRounds = 100;
            while (!_finished && maxRounds > 0)
            {
                maxRounds--;
                Thread.Sleep(100);
            }

            if (!_finished)
            {
                throw new Exception($"Timed out. Intermediate result ${_actualWriter.GetLongAnimationSequence()}");
            }

            // The writer gives us a string that describes the order in which he animated the objects.
            var result = _actualWriter.GetAnimationSequence();

            return result;

        }

        private void NextTask()
        {
            if (!_finished)
            {
                var task = _actualConsumer.HandleNextScreenObjectAsync();
                task.ContinueWith((t) =>
                {
                    // When the animation of the object is finshed, the method 'WhenWriterIsFinished' is called.
                    NextTask();
                });
            }
        }

        private void WhenWriterIsFinished(int groupId, int objectId)
        {
            Debug.WriteLine($"AnimationHandlingTest: {objectId} is finished");
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
                    var waitForCompletedMovementsOfAnotherGroup = 0;
                    if (toParse.StartsWith("??"))
                    {
                        startWhenPredecessorHasFinished = true;
                        toParse = toParse[2..];
                        if (toParse.StartsWith('('))
                        {
                            var indexOfClosingPara = toParse.IndexOf(')');
                            waitForCompletedMovementsOfAnotherGroup = int.Parse(toParse[1..indexOfClosingPara]);
                            toParse = toParse[(indexOfClosingPara + 1)..];
                        }
                        else
                        {
                            throw new Exception("If animationAsString starts with ?? it must contain anothger group id");
                        }
                    }
                    else if (toParse.StartsWith('?'))
                    {
                        toParse = toParse[1..];
                        if (toParse.StartsWith('('))
                        {
                            var indexOfClosingPara = toParse.IndexOf(')');
                            waitForCompletedMovementsOfAnotherGroup = int.Parse(toParse[1..indexOfClosingPara]);
                            toParse = toParse[(indexOfClosingPara + 1)..];
                        }
                        else
                        {
                            startWhenPredecessorHasFinished = true;
                        }
                    }
                    var indexOfComma = toParse.IndexOf(',');
                    var groupId = int.Parse(toParse[..indexOfComma]);

                    toParse = toParse[(indexOfComma + 1)..];
                    var indexOfDoubleColon = toParse.IndexOf(':');
                    var objectId = int.Parse(toParse[..indexOfDoubleColon]);
                    var duration = int.Parse(toParse[(indexOfDoubleColon + 1)..]);

                    AddAnimatedObject(groupId, objectId, duration, startWhenPredecessorHasFinished, waitForCompletedMovementsOfAnotherGroup);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in AddAnimatedSequence");
                Debug.WriteLine("\nMessage ---\n{0}", ex.Message);
                Debug.WriteLine("\nHelpLink ---\n{0}", ex.HelpLink);
                Debug.WriteLine("\nSource ---\n{0}", ex.Source);
                Debug.WriteLine("\nStackTrace ---\n{0}", ex.StackTrace);
                Debug.WriteLine("\nTargetSite ---\n{0}", ex.TargetSite);
            }
        }


        private void AddAnimatedObject(int groupId, int objectId, int duration, bool startWhenPredecessorHasFinished, int otherGroupId)
        {
            var line = new ScreenLine()
            {
                ID = objectId,
                GroupID = groupId,
            };

            if (startWhenPredecessorHasFinished)
            {
                line.WaitForCompletedAnimationsOfSameGroup = true;
            }

            if (otherGroupId != 0)
                line.WaitForCompletedAnimationsOfAnotherGroup = otherGroupId;

            if (duration > 0)
            {
                line.Animation = new ScreenAnimation();

                line.Animation.Effects.Add(new ScreenAnimationEffect() { Milliseconds = duration });
            }

            _actualProducer.DrawLine(line);
        }

    }


    internal record AnimationProtocolEntry(int ID, bool Finished, int Milliseconds);

    /// <summary>
    /// Mockup for the test
    /// </summary>
    internal class ScreenWriterMockup : IScreenObjectWriter
    {
        private readonly List<AnimationProtocolEntry> _animationProtocol = new();

        public void UpdateWithAnimation(ScreenObject screenObject)
        {
            // protocol the start of the animation
            _animationProtocol.Add(new AnimationProtocolEntry(screenObject.ID, false, (screenObject.Animation != null) ? screenObject.Animation.Milliseconds : 0));

            var animation = screenObject.Animation;
            var thread = new Thread(
                new ThreadStart(() =>
                {
                    // simulate the animation
                    Thread.Sleep(animation.Milliseconds);

                    // animation is finished. protocol it
                    _animationProtocol.Add(new AnimationProtocolEntry(screenObject.ID, true, animation.Milliseconds));

                    // Inform everyone who wants to know that the animation is finished
                    OnAnimationIsFinished(screenObject.GroupID, screenObject.ID);
                }
                )
            );
            thread.Start();
        }

        /// <summary>
        /// The Writer calls these events for every animation which is finished
        /// </summary>
        public event AnimationIsFinished OnAnimationIsFinished;

        public void Update(ScreenObject screenObject)
        {
            _animationProtocol.Add(new AnimationProtocolEntry(screenObject.ID, false, (screenObject.Animation != null) ? screenObject.Animation.Milliseconds : 0));
            _animationProtocol.Add(new AnimationProtocolEntry(screenObject.ID, true, (screenObject.Animation != null) ? screenObject.Animation.Milliseconds : 0));
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

        public string GetLongAnimationSequence()
        {
            var sb = new StringBuilder();
            foreach (var entry in _animationProtocol)
            {
                if (entry.Finished)
                {
                    sb.Append($"<{entry.ID}_{entry.Milliseconds}]");
                }
                else
                {
                    sb.Append($"[{entry.ID}_{entry.Milliseconds}>");
                }
            }
            return sb.ToString();
        }

        public void SetScreenResultProducer(IScreenResultProducer producer)
        {
            // Ignore
        }
    }

}
