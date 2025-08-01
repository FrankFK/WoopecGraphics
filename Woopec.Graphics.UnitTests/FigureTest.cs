﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woopec.Graphics.Helpers;
using Woopec.Graphics.Internal.Communication;
using Woopec.Graphics.Interface.Dtos;
using Woopec.Graphics.Interface.Screen;

namespace Woopec.Graphics.UnitTests
{

    [TestClass]
    public class FigureTest
    {
        private class ScreenMockup : ILowLevelScreen
        {
            public int FigureCounter;

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
                FigureCounter++;
                return FigureCounter;
            }

            public void UpdateFigure(ScreenFigure figure)
            {
                FigureUpdates.Add(figure);
            }


            public Task<string> TextInputAsync(string title, string prompt, DtoVec2D position)
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

            public Task<DtoVec2D> ShowTextBlockWithReturnCoordinateAsync(ScreenTextBlock textBlock)
            {
                throw new NotImplementedException();
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


        /// <summary>
        /// When a figure is created, it is invisible for the time being.
        /// The user should be able to decide when and where to see it first.
        /// </summary>
        [TestMethod]
        public void Figure_StandardCreated_IsInvisible()
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
            screenMockup.FigureUpdates[0].FillColor.Should().Be(DtoMapper.Map(initialColor));  // figure is set to visible. Fill color must be initial value
            screenMockup.FigureUpdates[1].FillColor.Should().Be(DtoMapper.Map(Colors.Red));    // Color changed
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
            screenMockup.FigureUpdates[0].OutlineColor.Should().Be(DtoMapper.Map(initialColor));  // figure is set to visible. Fill color must be initial value
            screenMockup.FigureUpdates[1].OutlineColor.Should().Be(DtoMapper.Map(Colors.Red));    // Color changed
        }

        [TestMethod]
        public void Figure_ShapeChangesAtTheBeginningProducesNoOverhead()
        {
            var screenMockup = new ScreenMockup();

            // Act
            var figure = new Figure(screenMockup) { Shape = Shapes.Turtle };

            // Assert. The creation of the figure did not sent events to the screen
            screenMockup.FigureCounter.Should().Be(0);
            screenMockup.FigureUpdates.Count.Should().Be(0);

            // Only when the figure is set to visible, the figure is created
            figure.IsVisible = true;
            screenMockup.FigureCounter.Should().Be(1);
            screenMockup.FigureUpdates.Count.Should().Be(1);
        }

        [TestMethod]
        public void Figure_ChangeOfShapeChangesShapeOnScreen()
        {
            var screenMockup = new ScreenMockup();

            // Act
            var figure = CreateSut(screenMockup);

            // The figure is shown with the default shape:
            figure.IsVisible = true;
            screenMockup.FigureCounter.Should().Be(1); // the default shape

            // A simple move
            figure.Move(50);

            // Because the shape did not change, it is not contained in the update
            screenMockup.FigureUpdates.Last().Shape.Should().BeNull();

            // Now we change the shape
            figure.Shape = Shapes.Turtle;

            // Because the shape has changed, it is contained in the update
            screenMockup.FigureUpdates.Last().Shape.Should().NotBeNull();
        }

        [TestMethod]
        public void Figure_AddShapeAndUseIt()
        {
            var screenMockup = new ScreenMockup();

            var shape = new Shape();
            shape.AddComponent(new() { (0, 0), (10, -5), (0, 10), (-10, -5) }, Colors.Blue, Colors.Yellow);
            shape.AddComponent(new() { (0, 0), (-10, 5), (0, -10), (10, 5) }, Colors.Yellow, Colors.Blue);

            Shapes.Add("compound shape", shape);

            var figure = CreateSut(screenMockup);
            figure.IsVisible = true;
            figure.Move(50);

            // Because the shape did not change, it is not contained in the update
            screenMockup.FigureUpdates.Last().Shape.Should().BeNull();

            // Act: Change the shape
            figure.Shape = Shapes.Get("compound shape");

            // Because the shape has changed, it is contained in the update
            screenMockup.FigureUpdates.Last().Shape.Should().NotBeNull();
        }

        [TestMethod]
        public void MultipleUsageOfSamePredefinedShape_DoesWork()
        {
            var screenMockup = new ScreenMockup();
            var figure1 = new Figure(screenMockup) { IsVisible = false, Shape = Shapes.Turtle };
            var figure2 = new Figure(screenMockup) { IsVisible = false, Shape = Shapes.Turtle };
        }

        private static Figure CreateSut(ILowLevelScreen screen)
        {
            return new Figure(screen);
        }
    }
}
