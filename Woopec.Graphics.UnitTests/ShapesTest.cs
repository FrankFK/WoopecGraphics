using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopec.Graphics;

namespace Woopec.Graphics.UnitTests
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
            Shapes.Add("polygon1", new List<Vec2D>() { (-10, 0), (10, 0), (0, 10) });
            Shapes.GetNames().Should().Contain("polygon1");
        }

        [TestMethod]
        public void AddCompoundShape()
        {
            var shape = new Shape();

            shape.AddComponent(new() { (0, 0), (10, -5), (0, 10), (-10, -5) }, Colors.Blue, Colors.Yellow);
            shape.AddComponent(new() { (0, 0), (-10, 5), (0, -10), (10, 5) }, Colors.Yellow, Colors.Blue);

            Shapes.Add("compound shape1", shape);
            Shapes.GetNames().Should().Contain("compound shape1");
        }

        [TestMethod]
        public void Add_WithASimplePointList_AddTheShapeWithItsName()
        {
            // arrange
            var shapeName = "polygon2";

            // act
            Shapes.Add(shapeName, new List<Vec2D>() { (-10, 0), (10, 0), (0, 10) });

            // assert
            Shapes.GetNames().Should().Contain("polygon2");
            var returnedShape = Shapes.Get(shapeName);
            returnedShape.Name.Should().Be(shapeName); // the user has had no choice to specify the namen when he has called Shapes.Add. Therefore Shapes.Add set the name internally.
        }

        [TestMethod]
        public void Add_WithAShape_DoesNotChangeTheShapesName()
        {
            // arrange
            var shapeName = "myNewCompound1";
            var shape = new Shape();

            shape.AddComponent(new() { (0, 0), (10, -5), (0, 10), (-10, -5) }, Colors.Blue, Colors.Yellow);
            shape.AddComponent(new() { (0, 0), (-10, 5), (0, -10), (10, 5) }, Colors.Yellow, Colors.Blue);
            Shapes.Add(shapeName, shape);

            Shapes.GetNames().Should().Contain(shapeName);

            // act
            var returnedShape = Shapes.Get(shapeName);

            // assert
            returnedShape.Name.Should().BeNullOrWhiteSpace(); // the user has had the choice to set shape.Name before he has called Shapes.Add. Therefore Shapes.Add did not change the name internally.
        }

        [TestMethod]
        public void Add_WithAShapeThatHasAName_PreservesTheName()
        {
            // arrange
            var shapeName = "myNewCompound2";
            var shape = new Shape() { Name = shapeName };

            shape.AddComponent(new() { (0, 0), (10, -5), (0, 10), (-10, -5) }, Colors.Blue, Colors.Yellow);
            shape.AddComponent(new() { (0, 0), (-10, 5), (0, -10), (10, 5) }, Colors.Yellow, Colors.Blue);
            Shapes.Add(shapeName, shape);

            Shapes.GetNames().Should().Contain(shapeName);

            // act
            var returnedShape = Shapes.Get(shapeName);

            // assert
            returnedShape.Name.Should().Be(shapeName);
        }

    }
}
