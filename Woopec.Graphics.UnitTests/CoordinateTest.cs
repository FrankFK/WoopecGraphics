using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;

namespace Woopec.Graphics.UnitTests
{
    [TestClass]
    public class CoordinateTest
    {

        [TestMethod]
        public void Coordinate_CreateByDouble()
        {
            // Usually you would have to create the Coordinate with a constructor:
            var coordinate1 = new Coordinate(1.0);
            coordinate1.Value.Should().Be(1.0);

            // Act: Coordinate has an implicit conversion from double to Coordinate. So you can do it that way:
            Coordinate coordinate2 = 1.0;

            // Assert
            coordinate2.Value.Should().Be(1.0);
        }


        [TestMethod]
        public void Coordinate_ConvertedToDouble()
        {
            // Arrange
            Coordinate coordinate = 2.0;

            // Act: Usually you would have to get the property coordinate.Value.
            //      Coordinate hat an implicit conversion to double. So you can do it that way:
            double value = coordinate;

            // Assert
            value.Should().Be(2.0);
        }

        [TestMethod]
        public void Coordinate_Comparison()
        {
            // Arrange
            Coordinate coor1 = 2.0;
            Coordinate coor2 = 1.0;

            // Act: The ">" operator could not be used if no conversion was defined.
            // Because a conversion to double is defined, the operator does the right thing.
            var comparison = (coor1 > coor2);

            // Assert
            comparison.Should().BeTrue();
        }

        [TestMethod]
        public void Coordinate_Addiditon()
        {
            // Arrange
            Coordinate coor1 = 2.0;
            Coordinate coor2 = 1.0;

            // Act: The "+" operator could not be used if no conversion was defined.
            // Because a conversion to double is defined, the operator does the right thing.
            var result = coor1 + coor2;

            // Assert
            var isDouble = (result.GetType() == typeof(double));
            isDouble.Should().BeTrue();
            result.Should().Be(3.0);
        }
    }
}
