using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TurtleCore.UnitTests
{
    [TestClass]
    public class ScreenTest
    {

        private class TurtleScreenProducerMockup : IScreenObjectProducer
        {
            public List<ScreenLine> DrawnLines = new();

            public int CreateLine()
            {
                return 1;
            }

            public void DrawLine(ScreenLine line)
            {
                DrawnLines.Add(line);
            }
        }

        private static TurtleScreenProducerMockup _producerMockup;

        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            _producerMockup = new TurtleScreenProducerMockup();
            TurtleOutputs.InitializeDefaultScreenObjectProducer(_producerMockup);
        }

        [TestCleanup]
        public void TestsCleanup()
        {
            Screen.ResetDefaultScreen();
        }

        [TestMethod]
        public void Screen_ThereArePrefefinedShapes()
        {
            var screen = Screen.GetDefaultScreen();
            var predefinedShapes = screen.GetShapes();
            predefinedShapes.Count.Should().BeGreaterThan(5);
            predefinedShapes.Should().Contain("turtle");
            predefinedShapes.Should().Contain("arrow");
            predefinedShapes.Should().Contain("circle");
            predefinedShapes.Should().Contain("square");
            predefinedShapes.Should().Contain("classic");
            predefinedShapes.Should().Contain("triangle");
        }

        [TestMethod]
        public void Screen_AddPolygonShape()
        {
            var screen = Screen.GetDefaultScreen();
            screen.AddShape("polygon", new Shape(new() { (-10, 0), (10, 0), (0, 10) }));
            screen.GetShapes().Should().Contain("polygon");
        }

        [TestMethod]
        public void Screen_AddCompoundShape()
        {
            var screen = Screen.GetDefaultScreen();

            var shape = new Shape();
            shape.AddComponent(new() { (0, 0), (10, -5), (0, 10), (-10, -5) }, Colors.Blue, Colors.Yellow);
            shape.AddComponent(new() { (0, 0), (-10, 5), (0, -10), (10, 5) }, Colors.Yellow, Colors.Blue);

            screen.AddShape("compound shape", shape);
            screen.GetShapes().Should().Contain("compound shape");
        }
    }
}
