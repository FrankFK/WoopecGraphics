using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Core;

namespace Woopec.Core.UnitTests
{
    [TestClass]
    public class ShapesTest
    {
        [TestMethod]
        public void ThereArePrefefinedShapes()
        {
            Shapes.Turtle.Name.Should().NotBeNullOrWhiteSpace();
            Shapes.Arrow.Name.Should().NotBeNullOrWhiteSpace();
            Shapes.Circle.Name.Should().NotBeNullOrWhiteSpace();
            Shapes.Square.Name.Should().NotBeNullOrWhiteSpace();
            Shapes.Classic.Name.Should().NotBeNullOrWhiteSpace();
            Shapes.Triangle.Name.Should().NotBeNullOrWhiteSpace();
            Shapes.Bird.Name.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void AddPolygonShape()
        {
            Shapes.Add("polygon", new List<Vec2D>() { (-10, 0), (10, 0), (0, 10) });
            Shapes.GetNames().Should().Contain("polygon");
        }

        [TestMethod]
        public void AddCompoundShape()
        {
            var shape = new Shape();
            shape.AddComponent(new() { (0, 0), (10, -5), (0, 10), (-10, -5) }, Colors.Blue, Colors.Yellow);
            shape.AddComponent(new() { (0, 0), (-10, 5), (0, -10), (10, 5) }, Colors.Yellow, Colors.Blue);

            Shapes.Add("compound shape", shape);
            Shapes.GetNames().Should().Contain("compound shape");
        }

    }
}
