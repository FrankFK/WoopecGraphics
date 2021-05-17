using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TurtleCore.UnitTests
{

    [TestClass]
    public class TurtleTest
    {
        private class ScreenMockup : IScreen
        {
            private int _figureCounter;
            private int _lineCounter;

            public List<ScreenFigure> FigureUpdates = new();
            public List<ScreenLine> DrawnLines = new();

            public int LastIssuedAnimatonGroupID { get; set; }


            public int CreateLine()
            {
                _lineCounter++;
                return _lineCounter;
            }

            public void DrawLine(ScreenLine line)
            {
                DrawnLines.Add(line);
            }

            public void RegisterShape(string name, ShapeBase shape)
            {
                throw new NotImplementedException();
            }
            public void AddShape(string name, ShapeBase shape)
            {
                throw new NotImplementedException();
            }
            public List<string> GetShapes()
            {
                throw new NotImplementedException();
            }


            public int CreateFigure(string shapeName)
            {
                _figureCounter++;
                return _figureCounter;
            }

            public void UpdateFigure(ScreenFigure figure)
            {
                FigureUpdates.Add(figure);
            }
        }

        [TestMethod]
        public void Turtle_InitialValues()
        {
            // Arrange

            // Act
            var turtle = CreateSut();

            // Assert
            var expected = new Vec2D(0, 0);
            turtle.Position.Should().Be(expected);
            turtle.Heading.Should().Be(0);

            // The turtle is immediately visible after creation!
            turtle.IsVisible.Should().BeTrue();
        }


        [TestMethod]
        public void Turtle_Forward_Works()
        {
            // Arrange
            var turtle = CreateSut();

            // Act
            turtle.Forward(25);

            // Assert
            var expected = new Vec2D(25, 0);
            turtle.Position.Should().Be(expected);

            // Act
            turtle.Forward(25);

            // Assert
            var expected2 = new Vec2D(50, 0);
            turtle.Position.Should().Be(expected2);
        }

        [TestMethod]
        public void Turtle_Backward_Works()
        {
            // Arrange
            var turtle = CreateSut();

            // Act
            turtle.Backward(25);

            // Assert
            var expected = new Vec2D(-25, 0);
            turtle.Position.Should().Be(expected);

            // Act
            turtle.Backward(25);

            // Assert
            var expected2 = new Vec2D(-50, 0);
            turtle.Position.Should().Be(expected2);
        }

        [TestMethod]
        public void Turtle_Left_Works()
        {
            // Arrange
            var turtle = CreateSut();

            // Act
            turtle.Left(90);

            // Assert
            turtle.Heading.Should().Be(90);

            turtle.Forward(100);
            var expextedPostion = new Vec2D(0, 100);
            turtle.Position.IsApproximatelyEqualTo(expextedPostion, 0.001).Should().BeTrue();
        }

        [TestMethod]
        public void Turtle_LeftWithMoreThan360Degrees_Works()
        {
            // Arrange
            var turtle = CreateSut();

            // Act
            turtle.Left(450);

            // Assert
            turtle.Heading.Should().Be(90);
        }

        [TestMethod]
        public void Turtle_Right_Works()
        {
            // Arrange
            var turtle = CreateSut();

            // Act
            turtle.Right(90);

            // Assert
            turtle.Heading.Should().Be(270);

            turtle.Forward(100);
            var expextedPostion = new Vec2D(0, -100);
            turtle.Position.IsApproximatelyEqualTo(expextedPostion, 0.001).Should().BeTrue();
        }

        [TestMethod]
        public void Turtle_InitiallyPenIsDown()
        {
            var screenMockup = new ScreenMockup();

            // Arrange
            var turtle = CreateSut(screenMockup);

            // Act
            turtle.Forward(100);

            // Assert
            screenMockup.DrawnLines.Count.Should().Be(1);
        }

        [TestMethod]
        public void Turtle_WhenPenIsUpNoLineIsDrawn()
        {
            var screenMockup = new ScreenMockup();

            // Arrange
            var turtle = CreateSut(screenMockup);

            // Act
            turtle.PenUp();
            turtle.Forward(100);

            // Assert
            screenMockup.DrawnLines.Count.Should().Be(0);
        }

        [TestMethod]
        public void Turtle_MoveAfterPenDownIsDrawn()
        {
            var screenMockup = new ScreenMockup();

            // Arrange
            var turtle = CreateSut(screenMockup);

            // Act
            turtle.PenUp();
            turtle.Forward(100);
            turtle.Left(90);
            turtle.PenDown();
            turtle.Forward(50);

            // Assert
            screenMockup.DrawnLines.Count.Should().Be(1);
            var line = screenMockup.DrawnLines[0];
            line.Point1.IsApproximatelyEqualTo(new Vec2D(100, 0), 0).Should().BeTrue();
            line.Point2.IsApproximatelyEqualTo(new Vec2D(100, 50), 0).Should().BeTrue();
        }

        [TestMethod]
        public void Turtle_PositionChangeWithPendownIsDrawn()
        {
            var screenMockup = new ScreenMockup();

            // Arrange
            var turtle = CreateSut(screenMockup);

            // Act
            turtle.PenDown();
            turtle.SetPosition(new Vec2D(0, 100));

            // Assert
            screenMockup.DrawnLines.Count.Should().Be(1);
        }

        [TestMethod]
        public void Turtle_InitiallyATurtleIsShown()
        {
            var screenMockup = new ScreenMockup();

            // Act
            var turtle = CreateSut(screenMockup);

            // Assert
            screenMockup.FigureUpdates.Count.Should().Be(1);
            screenMockup.FigureUpdates[0].IsVisible.Should().BeTrue();
        }


        private static Turtle CreateSut()
        {
            return new Turtle(new ScreenMockup());
        }

        private static Turtle CreateSut(IScreen screen)
        {
            return new Turtle(screen);
        }
    }
}
