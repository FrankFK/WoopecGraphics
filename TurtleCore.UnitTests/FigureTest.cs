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

            public int LastIssuedAnimatonGroupID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


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



        private static Figure CreateSut(IScreen screen)
        {
            return new Figure(screen);
        }
    }
}
