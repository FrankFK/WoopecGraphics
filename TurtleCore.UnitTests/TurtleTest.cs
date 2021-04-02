using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TurtleCore.UnitTests
{
    [TestClass]
    public class TurtleTest
    {
        [TestMethod]
        public void Turtle_StartPosition_Is_Null()
        {
            // Arrange

            // Act
            var turtle = new Turtle();

            // Assert
            var expected = new Vec2D(0, 0);
            turtle.Position.Should().Be(expected);
        }

        [TestMethod]
        public void Turtle_Forward_Works()
        {
            // Arrange
            var turtle = new Turtle();

            // Act
            turtle.Forward(25);

            // Assert
            var expected = new Vec2D(25, 0);
            turtle.Position.Should().Be(expected);

            // Act
            turtle.Forward(25);

            // Assert
            var expected2 = new Vec2D(50, 0);
            turtle.Position.Should().Be(expected2);
        }

    }
}
