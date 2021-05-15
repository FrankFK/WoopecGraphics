using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TurtleCore.UnitTests
{

    [TestClass]
    public class FigureTest
    {
        private class TurtleScreenProducerMockup : IScreenObjectProducer
        {
            public List<ScreenLine> DrawnLines = new();
            public List<ScreenFigure> FigureUpdates = new();

            private int _figureCounter = 0;

            public int CreateLine()
            {
                return 1;
            }

            public void DrawLine(ScreenLine line)
            {
                DrawnLines.Add(line);
            }
            public int CreateFigure(ShapeBase shape)
            {
                _figureCounter++;
                return _figureCounter - 1;
            }

            public void UpdateFigure(ScreenFigure figure)
            {
                FigureUpdates.Add(figure);
            }


        }

        private static TurtleScreenProducerMockup _producerMockup;

        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            Console.WriteLine("Init start");
            _producerMockup = new TurtleScreenProducerMockup();
            TurtleOutputs.InitializeDefaultScreenObjectProducer(_producerMockup);
            Console.WriteLine("Init end");
        }

        [TestCleanup]
        public void TestsCleanup()
        {
            Screen.ResetDefaultScreen();
        }


        [TestMethod]
        public void Figure_InitialValues()
        {
            // Arrange

            // Act
            var figure = CreateSut();

            // Assert
            figure.IsVisible.Should().BeFalse();
        }



        private static Figure CreateSut()
        {
            return new Figure();
        }
    }
}
