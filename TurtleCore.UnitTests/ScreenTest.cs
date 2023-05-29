using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Woopec.Core.Internal;

namespace Woopec.Core.UnitTests
{
    [TestClass]
    public class ScreenTest
    {

        private class TurtleScreenProducerMockup : IScreenObjectProducer
        {
            public int CreateLine()
            {
                throw new NotImplementedException();
            }
            public void DrawLine(ScreenLine line)
            {
                throw new NotImplementedException();
            }

            public int CreateFigure()
            {
                throw new NotImplementedException();
            }

            public void UpdateFigure(ScreenFigure figure)
            {
                throw new NotImplementedException();
            }

            public void ShowDialog(ScreenDialog dialog)
            {
                throw new NotImplementedException();
            }

            public void ShowNumberDialog(ScreenNumberDialog dialog)
            {
                throw new NotImplementedException();
            }
        }

        private class TurtleScreenResultConsumerMockup : IScreenResultConsumer
        {
            public Task<double?> ReadNumberResultAsync()
            {
                throw new NotImplementedException();
            }

            public Task<string> ReadTextResultAsync()
            {
                throw new NotImplementedException();
            }
        }

        private static TurtleScreenProducerMockup _producerMockup;
        private static TurtleScreenResultConsumerMockup _resultConsumerMockup;

        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            _producerMockup = new TurtleScreenProducerMockup();
            _resultConsumerMockup = new TurtleScreenResultConsumerMockup();
            TurtleInputsAndOutputs.InitializeDefaultScreenObjectProducer(_producerMockup);
            TurtleInputsAndOutputs.InitializeDefaultScreenResultConsumer(_resultConsumerMockup);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            LowLevelDefaultScreen.Reset();
        }

        /// <summary>
        /// The method SwitchToUnitTestDefaultScreen makes it possible to easily use Turtle, Figure and Pen objects in Unit Tests.
        /// This method shows what happens, if a Unit Test ist implemented without using SwitchToUnitTestDefaultScreen.
        /// </summary>
        [TestMethod]
        public void SwitchToUnitTestDefaultScreen_WithoutCallOfIt_WoopecObjectsCanNotBeUsedInUnitTests()
        {
            // arrange:
            // This is simulating a unit test context without the UnitTestDefaultScreen
            TurtleInputsAndOutputs.InitializeDefaultScreenObjectProducer(null);
            TurtleInputsAndOutputs.InitializeDefaultScreenResultConsumer(null);

            // act:
            // We are using the default screen which is not set up correctly in unit test scenarios

            // assert
            // In this case we expect an exception.
            bool exception = false;
            try
            {
                var figure = new Figure() { IsVisible = true };
                figure.Position = (10, 10);
            }
            catch (Exception ex)
            {
                exception = true;
                var expected = nameof(Screen.SwitchToUnitTestDefaultScreen);
                ex.Message.Should().Contain(expected, "Woopec should create an exception text the direct the user to a solution.");
            }
            finally
            {
                // Set everything back to normal mockups
                TurtleInputsAndOutputs.InitializeDefaultScreenObjectProducer(_producerMockup);
                TurtleInputsAndOutputs.InitializeDefaultScreenResultConsumer(_resultConsumerMockup);
            }

            Assert.IsTrue(exception);
        }

        /// <summary>
        /// The method SwitchToUnitTestDefaultScreen makes it possible to easily use Turtle, Figure and Pen objects in Unit Tests.
        /// This method shows that a Unit Test works without problems if SwitchToUnitTestDefaultScreen was called at start of the test.
        /// </summary>
        /// <remarks>
        /// To restore the normal state, method SwitchToUnitTestDefaultScreen should be called after the unit test.
        /// </remarks>
        [TestMethod]
        public void SwitchToUnitTestDefaultScreen_AfterCallOfIt_WoopecObjectsCanBeUsedInUnitTests()
        {
            // act:
            Screen.SwitchToUnitTestDefaultScreen();

            // assert: No exceptions when using woopec objects
            var figure = new Figure() { IsVisible = true };
            figure.Position = (10, 10);

            var turtle = new Turtle();
            turtle.Right(90);
            turtle.Forward(100);

            var pen = new Pen() { IsDown = true };
            pen.SetPosition((10, 10));

            // clean up
            Screen.SwitchToNormalDefaultScreen();
        }

        /// <summary>
        /// The method SwitchToNormalDefaultScreen resets the default screen back to its normal behaviour.
        /// </summary>
        [TestMethod]
        public void SwitchToNormalDefaultScreen_AfterCallOfIt_WoopecObjectsCannotBeUsedInUnitTests()
        {
            // arrange:
            Screen.SwitchToUnitTestDefaultScreen();

            var figure = new Figure() { IsVisible = true };
            figure.Position = (10, 10);

            // act:
            Screen.SwitchToNormalDefaultScreen();

            // assert:
            bool exception = false;
            try
            {
                var newFigure = new Figure() { IsVisible = true };
            }
            catch (Exception)
            {
                exception = true;
            }
            Assert.IsTrue(exception);
        }
    }
}
