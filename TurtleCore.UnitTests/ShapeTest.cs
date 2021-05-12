using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;

namespace TurtleCore.UnitTests
{
    [TestClass]
    public class ShapeTest
    {

        [TestMethod]
        public void Shape_CreatePolygon()
        {
            // Act
            var shape = new Shape(new() { (0, 0), (10, -5), (0, 10), (-10, -5) });

            // Assert
            shape.Type.Should().Be(ShapeType.Polygon);
            shape.Components.Count.Should().Be(1);
        }

        [TestMethod]
        public void Shape_CreateCompoundWithoutOutlineColor()
        {
            // Act
            var shape = new Shape();
            shape.AddComponent(new() { (0, 0), (10, -5), (0, 10), (-10, -5) }, Colors.Blue);

            // Assert
            shape.Type.Should().Be(ShapeType.Compound);
            shape.Components.Count.Should().Be(1);
            shape.Components[0].Polygon.Count.Should().Be(4);
            (shape.Components[0].Polygon[0] is Vec2D).Should().BeTrue();
            shape.Components[0].FillColor.Should().Be(Colors.Blue);
            shape.Components[0].OutlineColor.Should().BeNull();
        }

        [TestMethod]
        public void Shape_CreateCompoundWithOutlineColor()
        {
            // Act
            var shape = new Shape();
            shape.AddComponent(new() { (0, 0), (10, -5), (0, 10), (-10, -5) }, Colors.Blue, Colors.Yellow);

            // Assert
            shape.Type.Should().Be(ShapeType.Compound);
            shape.Components.Count.Should().Be(1);
            shape.Components[0].Polygon.Count.Should().Be(4);
            (shape.Components[0].Polygon[0] is Vec2D).Should().BeTrue();
            shape.Components[0].FillColor.Should().Be(Colors.Blue);
            shape.Components[0].OutlineColor.Should().Be(Colors.Yellow);
        }

        [TestMethod]
        public void Shape_CreateCompoundWithTwoPolygons()
        {
            // Act
            var shape = new Shape();
            shape.AddComponent(new() { (0, 0), (10, -5), (0, 10), (-10, -5) }, Colors.Blue, Colors.Yellow);
            shape.AddComponent(new() { (0, 0), (-10, 5), (0, -10), (10, 5) }, Colors.Yellow, Colors.Blue);

            // Assert
            shape.Type.Should().Be(ShapeType.Compound);
            shape.Components.Count.Should().Be(2);
        }

        [TestMethod]
        public void Shape_AddComponentToPolygonShapeYieldsException()
        {
            // Act
            var shape = new Shape(new() { (0, 0), (10, -5), (0, 10), (-10, -5) });

            Action act = () => shape.AddComponent(new() { (0, 0), (10, -5), (0, 10), (-10, -5) }, Colors.Blue, Colors.Yellow); ;

            act.Should().Throw<NotSupportedException>();
        }

        [TestMethod]
        public void Shape_CreateImageShapeIsNotPossibleAtTheMoment()
        {
            // Act
            Action act = () => { var shape = new ImageShape("a/file/path"); };

            act.Should().Throw<NotImplementedException>();
        }

    }
}
