using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TurtleCore.UnitTests
{

    [TestClass]
    public class FigureTest
    {
        private class ScreenMockup : IScreen
        {
            private int _figureCounter;

            public List<ScreenFigure> FigureUpdates = new();

            public int LastIssuedAnimatonGroupID { get; set; }


            public int CreateLine()
            {
                throw new NotImplementedException();
            }

            public void DrawLine(ScreenLine line)
            {
                throw new NotImplementedException();
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

        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
        }

        [TestCleanup]
        public void TestsCleanup()
        {
        }


        [TestMethod]
        public void Figure_InitialValues()
        {
            // Arrange
            var screenMockup = new ScreenMockup();

            // Act
            var figure = CreateSut(screenMockup);

            // Assert
            figure.IsVisible.Should().BeFalse();
        }

        [TestMethod]
        public void Figure_CreatedByATurtleIsImmediatelyShown()
        {
            var screenMockup = new ScreenMockup();

            // Act
            var turtle = new Turtle(screenMockup);

            // Assert
            turtle.Figure.IsVisible.Should().BeTrue();
            screenMockup.FigureUpdates.Count.Should().Be(1);
            screenMockup.FigureUpdates[0].IsVisible.Should().BeTrue();
        }


        [TestMethod]
        public void Figure_SetFillColorWhenFigureIsNotVisible_ChangesNothing()
        {
            var screenMockup = new ScreenMockup();
            var figure = CreateSut(screenMockup);

            // Act
            figure.FillColor = Colors.Red;

            // Assert
            figure.FillColor.Should().Be(Colors.Red);
            // The figure is not visible, therefore nothing an the screen should be changed.
            screenMockup.FigureUpdates.Count.Should().Be(0);
        }

        [TestMethod]
        public void Figure_SetFillColorWhenFigureIsVisible_ChangesScreenColor()
        {
            var screenMockup = new ScreenMockup();
            var figure = CreateSut(screenMockup);

            // Act
            var initialColor = figure.FillColor;
            figure.IsVisible = true;
            figure.FillColor = Colors.Red;

            // Assert
            figure.FillColor.Should().Be(Colors.Red);
            screenMockup.FigureUpdates.Count.Should().Be(2);
            screenMockup.FigureUpdates[0].FillColor.Should().Be(initialColor);  // figure is set to visible. Fill color must be initial value
            screenMockup.FigureUpdates[1].FillColor.Should().Be(Colors.Red);    // Color changed
        }


        [TestMethod]
        public void Figure_SetOutlineColorWhenFigureIsNotVisible_ChangesNothing()
        {
            var screenMockup = new ScreenMockup();
            var figure = CreateSut(screenMockup);

            // Act
            figure.OutlineColor = Colors.Red;

            // Assert
            figure.OutlineColor.Should().Be(Colors.Red);
            // The figure is not visible, therefore nothing an the screen should be changed.
            screenMockup.FigureUpdates.Count.Should().Be(0);
        }

        [TestMethod]
        public void Figure_SetOutlineColorWhenFigureIsVisible_ChangesScreenColor()
        {
            var screenMockup = new ScreenMockup();
            var figure = CreateSut(screenMockup);

            // Act
            var initialColor = figure.OutlineColor;
            figure.IsVisible = true;
            figure.OutlineColor = Colors.Red;

            // Assert
            figure.OutlineColor.Should().Be(Colors.Red);
            screenMockup.FigureUpdates.Count.Should().Be(2);
            screenMockup.FigureUpdates[0].OutlineColor.Should().Be(initialColor);  // figure is set to visible. Fill color must be initial value
            screenMockup.FigureUpdates[1].OutlineColor.Should().Be(Colors.Red);    // Color changed
        }



        private static Figure CreateSut(IScreen screen)
        {
            return new Figure(screen);
        }
    }
}
