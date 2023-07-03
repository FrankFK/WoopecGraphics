using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woopec.Core.Internal;

namespace Woopec.Core.UnitTests
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
        private class ScreenMockup : ILowLevelScreen
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
            public ShapeBase GetShape(string shapeName)
            {
                return new Shape();
            }

            public List<string> GetShapes()
            {
                throw new NotImplementedException();
            }


            public int CreateFigure()
            {
                _figureCounter++;
                return _figureCounter;
            }

            public void UpdateFigure(ScreenFigure figure)
            {
                FigureUpdates.Add(figure);
            }

            public Task<string> TextInputAsync(string title, string prompt, Vec2D position)
            {
                throw new NotImplementedException();
            }

            public Task<double?> NumberInputAsync(ScreenNumberDialog dialog)
            {
                throw new NotImplementedException();
            }

            public void ShowTextBlock(ScreenTextBlock textBlock)
            {
                throw new NotImplementedException();
            }

            public Task<Vec2D> ShowTextBlockWithReturnCoordinateAsync(ScreenTextBlock textBlock)
            {
                throw new NotImplementedException();
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

        /// <summary>
        /// When a turtle is created, it's pen is down.
        /// This is the default behaviour in python and therefore it is the default behaviour in Woopec, too.
        /// </summary>
        [TestMethod]
        public void Turtle_InitiallyPenIsDown()
        {
            var screenMockup = new ScreenMockup();

            // Arrange and act:
            var turtle = CreateSut(screenMockup);

            // Assert
            turtle.IsDown.Should().BeTrue();
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

        [TestMethod]
        public void Turtle_SetPositionViaSetter()
        {
            var screenMockup = new ScreenMockup();

            // Arrange
            var turtle = CreateSut(screenMockup);

            // Act
            turtle.Position = (0, 100);

            // Assert
            var expextedPostion = new Vec2D(0, 100);
            turtle.Position.IsApproximatelyEqualTo(expextedPostion, 0.001).Should().BeTrue();
            turtle.Pen.Position.IsApproximatelyEqualTo(expextedPostion, 0.001).Should().BeTrue();
            turtle.Figure.Position.IsApproximatelyEqualTo(expextedPostion, 0.001).Should().BeTrue();
        }

        [TestMethod]
        public void Turtle_SetPositionWithGoTo()
        {
            var screenMockup = new ScreenMockup();

            // Arrange
            var turtle = CreateSut(screenMockup);

            // Act
            turtle.GoTo((0, 100));

            // Assert
            var expextedPostion = new Vec2D(0, 100);
            turtle.Position.IsApproximatelyEqualTo(expextedPostion, 0.001).Should().BeTrue();
            turtle.Pen.Position.IsApproximatelyEqualTo(expextedPostion, 0.001).Should().BeTrue();
            turtle.Figure.Position.IsApproximatelyEqualTo(expextedPostion, 0.001).Should().BeTrue();
        }

        [TestMethod]
        public void Turtle_SetPenColor_ChangesPenAndFigure()
        {
            var screenMockup = new ScreenMockup();

            // Arrange
            var turtle = CreateSut(screenMockup);

            // Act
            turtle.PenColor = Colors.Red;

            // Assert
            turtle.Pen.Color.Should().Be(Colors.Red);
            turtle.Figure.OutlineColor.Should().Be(Colors.Red);
            turtle.PenColor.Should().Be(Colors.Red);
        }

        [TestMethod]
        public void Turtle_SetFillColor_ChangesFigure()
        {
            var screenMockup = new ScreenMockup();

            // Arrange
            var turtle = CreateSut(screenMockup);

            // Act
            turtle.FillColor = Colors.Red;

            // Assert
            turtle.Figure.FillColor.Should().Be(Colors.Red);
        }

        [TestMethod]
        public void Turtle_SetColor_ChangesPenColorAndFillColor()
        {
            var screenMockup = new ScreenMockup();

            // Arrange
            var turtle = CreateSut(screenMockup);

            // Act
            turtle.Color = Colors.Red;

            // Assert
            turtle.FillColor.Should().Be(Colors.Red);
            turtle.PenColor.Should().Be(Colors.Red);
        }

        [TestMethod]
        public void Turtle_Filling()
        {
            var screenMockup = new ScreenMockup();

            // Arrange
            var turtle = CreateSut(screenMockup);

            // Act
            turtle.IsDown = true;
            turtle.Forward(90);   // filling shape starts at 90, 0
            turtle.BeginFill();
            turtle.Filling.Should().BeTrue();
            turtle.SetPosition((90, 90));      // second point is 90, 90
            foreach (var _ in Enumerable.Range(0, 3))
            {
                turtle.Forward(90);
                turtle.Right(90);
            }
            turtle.EndFill();

            // Assert
            turtle.Filling.Should().BeFalse();
            var lastUpdate = screenMockup.FigureUpdates.Last();
            lastUpdate.Shape.Should().NotBeNull();
            lastUpdate.Shape.Type.Should().Be(ShapeType.Polygon);
            var shape = lastUpdate.Shape as Shape;
            var polygon = shape.Components[0].Polygon;
            polygon.Count.Should().Be(5);
        }


        private static Turtle CreateSut()
        {
            return new Turtle(new ScreenMockup());
        }

        private static Turtle CreateSut(ILowLevelScreen screen)
        {
            return new Turtle(screen);
        }
    }
}
