using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Woopec.Core.UnitTests
{
    [TestClass]
    public class Vec2DTest
    {

        [TestMethod]
        public void Vectors_With_Same_Values_Are_Equal()
        {
            // Arrange
            var vector1 = new Vec2D(1, 2);
            var vector2 = new Vec2D(1, 2);

            // Act
            var areEqual = (vector1 == vector2);

            // Assert
            areEqual.Should().BeTrue();
        }


        [TestMethod]
        public void Add_TwoVectors_Works()
        {
            // Arrange
            var vector1 = new Vec2D(1, 2);
            var vector2 = new Vec2D(4, 5);

            // Act
            var result = vector1 + vector2;

            // Assert
            var expected = new Vec2D(5, 7);
            result.Should().Be(expected);
        }

        [TestMethod]
        public void Subtract_TwoVectors_Works()
        {
            // Arrange
            var vector1 = new Vec2D(1, 2);
            var vector2 = new Vec2D(4, 5);

            // Act
            var result = vector1 - vector2;

            // Assert
            var expected = new Vec2D(-3, -3);
            result.Should().Be(expected);
        }

        [TestMethod]
        public void Multiply_TwoVectors_Works()
        {
            // Arrange
            var vector1 = new Vec2D(2, 3);
            var vector2 = new Vec2D(4, 5);

            // Act
            var result = vector1 * vector2;

            // Assert
            var expected = new Vec2D(8, 15);
            result.Should().Be(expected);
        }

        [TestMethod]
        public void ScalarMultiply_Vector_Works()
        {
            // Arrange
            var vector = new Vec2D(2, 3);

            // Act
            var result = 4 * vector;

            // Assert
            var expected = new Vec2D(8, 12);
            result.Should().Be(expected);
        }

        [TestMethod]
        public void Negate_Vector_Works()
        {
            // Arrange
            var vector = new Vec2D(2, 3);

            // Act
            var result = -vector;

            // Assert
            var expected = new Vec2D(-2, -3);
            result.Should().Be(expected);
        }

        [TestMethod]
        public void Absolute_Of_Vector_Works()
        {
            // Arrange
            var vector = new Vec2D(3, 4);

            // Act
            var result = vector.AbsoluteValue;

            // Assert
            result.Should().Be(5);
        }

        [TestMethod]
        public void Rotate_Vector_Works()
        {
            // Arrange
            var vector = new Vec2D(3, 4);

            // Act
            var result = vector.Rotate(90);

            // Assert
            var expected = new Vec2D(-4, 3);
            // There may be rounding differences between vector and expected. Therefore we check if the differences are nearly 0:
            (expected.X - result.X).Should().BeApproximately(0, 0.001);
            (expected.Y - result.Y).Should().BeApproximately(0, 0.001);
        }

        [TestMethod]
        public void HeadingTo_East()
        {
            // Arrange
            var vector = new Vec2D(3, 4);

            // Act
            var result = vector.HeadingTo((5, 4));

            // Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void HeadingTo_North()
        {
            // Arrange
            var vector = new Vec2D(3, 4);

            // Act
            var result = vector.HeadingTo((3, 6));

            // Assert
            result.Should().Be(90);
        }

        [TestMethod]
        public void HeadingTo_West()
        {
            // Arrange
            var vector = new Vec2D(3, 4);

            // Act
            var result = vector.HeadingTo((2, 4));

            // Assert
            result.Should().Be(180);
        }

        [TestMethod]
        public void HeadingTo_South()
        {
            // Arrange
            var vector = new Vec2D(3, 4);

            // Act
            var result = vector.HeadingTo((3, 2));

            // Assert
            result.Should().Be(270);
        }


        [TestMethod]
        public void ApproximatelyEqualVectors_AreRecognized()
        {
            var vector = new Vec2D(3.000, 4.000);
            var compare = new Vec2D(3.00099, 3.9990);

            vector.IsApproximatelyEqualTo(compare, 0.001).Should().BeTrue();
        }

        [TestMethod]
        public void ApproximatelyNonEqualVectors_AreRecognized()
        {
            var vector = new Vec2D(3.000, 4.000);
            var compare = new Vec2D(3.00101, 3.9980);

            vector.IsApproximatelyEqualTo(compare, 0.001).Should().BeFalse();
        }

        [TestMethod]
        public void Vector_IsACSharpRecord()
        {
            var vector = new Vec2D(3.000, 4.000);

            // Vector is immutable, value of property can not be changed:
            // vector.YCor = 0;

            // If I need a vector with a different value of a property, I can use a with statement:
            var vectorCopy = vector with { Y = 0 };
            var xCorOfCopy = vectorCopy.X;
            xCorOfCopy.Should().Be(3.000);

            // The original vector is not changed:
            var originalYCor = vector.Y;
            originalYCor.Should().Be(4.000);
        }

        [TestMethod]
        public void Vector_ToString_CreatesEasyToReadString()
        {
            var vector = new Vec2D(3, 4);

            // Only relevant values are serialized to a string, the property AbsoluteValue is omitted. This makes reading in debugger easier.
            Assert.IsTrue(vector.ToString() == "X = 3, Y = 4");
        }

        [TestMethod]
        public void Vector_RecordPerformanceIsFine()
        {
            const int Count = 100000;
            var result = 0;
            var timer = Stopwatch.StartNew();
            foreach (var i in Enumerable.Range(1, Count))
            {
                var vector1 = new Vec2D(i, 2.000);
                var vector2 = new Vec2D(i, 2.000);
                result += (vector1 == vector2) ? 1 : 0;
            }
            timer.Stop();
            var ticksPerCall = timer.ElapsedTicks / (double)Count;
            ticksPerCall.Should().BeLessThan(5);
        }


        [TestMethod]
        public void Vector_ConstructedByTuple()
        {
            Vec2D test = (1, 4);

            test.X.Should().Be(1);
            test.Y.Should().Be(4);
        }

        [TestMethod]
        public void VectorList_ConstructedByTuples()
        {
            List<Vec2D> list = new() { (1, 2), (3, 5) };

            list[0].X.Should().Be(1);
            list[1].X.Should().Be(3);
        }

        /// <summary>
        /// In python the programmer can add a polygon to a turtle shape with this code
        ///     shape.addcomponent( ((0,0),(10,-5),(0,10),(-10,-5)), "red", "blue"
        /// I tried to make it as simple as possible in C#. 
        /// I found no way to get rid of the "new()".
        /// </summary>
        [TestMethod]
        public void VectorList_ConstructedByTuplesAsMethodParam()
        {
            var result = AddComponent(new() { (1, 2), (3, 5) }, "red", "blue");

            result.Should().Be(1);
        }


        private static double AddComponent(List<Vec2D> list, string color1, string color2)
        {
            if (color1 != color2 && list.Count > 0)
                return list[0].X;
            else
                return 42;
        }

    }
}
