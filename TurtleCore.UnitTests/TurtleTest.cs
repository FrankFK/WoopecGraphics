using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TurtleCore.UnitTests
{

    /// <summary>
    /// Tests for the Turtle class
    /// </summary>
    /// <remarks>
    /// The Turtle class mainly is a container consisting of a Pen class and a Figure class.
    /// The most unit tests are implemented in PenTest and in FigureTest.
    /// Tests in TurtleTest only test the specifics of the Turtle class
    /// </remarks>
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

            //
            // We do not check all initial values of Pen and Figure here
            //
            // We only check values that are special to Turtle class or that are special when Pen or Figure are used as part of a Turtle
            //
            // The turtle is immediately visible after creation!
            turtle.IsVisible.Should().BeTrue();
            turtle.Figure.IsVisible.Should().BeTrue();
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
            turtle.Figure.Position.Should().Be(expected);
            turtle.Pen.Position.Should().Be(expected);
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
            turtle.Figure.Position.Should().Be(expected);
            turtle.Pen.Position.Should().Be(expected);
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
            turtle.Pen.Heading.Should().Be(90);
            turtle.Figure.Heading.Should().Be(90);

            turtle.Forward(100);
            var expextedPostion = new Vec2D(0, 100);
            turtle.Position.IsApproximatelyEqualTo(expextedPostion, 0.001).Should().BeTrue();
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
            turtle.Figure.Heading.Should().Be(270);
            turtle.Pen.Heading.Should().Be(270);
        }


        [TestMethod]
        public void Turtle_SetPosition()
        {
            var screenMockup = new ScreenMockup();

            // Arrange
            var turtle = CreateSut(screenMockup);

            // Act
            turtle.SetPosition(new Vec2D(0, 100));

            // Assert
            var expextedPostion = new Vec2D(0, 100);
            turtle.Position.IsApproximatelyEqualTo(expextedPostion, 0.001).Should().BeTrue();
            turtle.Pen.Position.IsApproximatelyEqualTo(expextedPostion, 0.001).Should().BeTrue();
            turtle.Figure.Position.IsApproximatelyEqualTo(expextedPostion, 0.001).Should().BeTrue();
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
