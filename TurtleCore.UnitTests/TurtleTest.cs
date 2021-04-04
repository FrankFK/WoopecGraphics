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
        public void Turtle_StartHeading_Is_Null()
        {
            // Arrange

            // Act
            var turtle = new Turtle();

            // Assert
            turtle.Heading.Should().Be(0);
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

        [TestMethod]
        public void Turtle_Backward_Works()
        {
            // Arrange
            var turtle = new Turtle();

            // Act
            turtle.Backward(25);

            // Assert
            var expected = new Vec2D(-25, 0);
            turtle.Position.Should().Be(expected);

            // Act
            turtle.Backward(25);

            // Assert
            var expected2 = new Vec2D(-50, 0);
            turtle.Position.Should().Be(expected2);
        }

        [TestMethod]
        public void Turtle_Left_Works()
        {
            // Arrange
            var turtle = new Turtle();

            // Act
            turtle.Left(90);

            // Assert
            turtle.Heading.Should().Be(90);

            turtle.Forward(100);
            var expextedPostion = new Vec2D(0, 100);
            turtle.Position.IsApproximatelyEqualTo(expextedPostion, 0.001).Should().BeTrue();
        }

        [TestMethod]
        public void Turtle_LeftWithMoreThan360Degrees_Works()
        {
            // Arrange
            var turtle = new Turtle();

            // Act
            turtle.Left(450);

            // Assert
            turtle.Heading.Should().Be(90);
        }

        [TestMethod]
        public void Turtle_Right_Works()
        {
            // Arrange
            var turtle = new Turtle();

            // Act
            turtle.Right(90);

            // Assert
            turtle.Heading.Should().Be(270);

            turtle.Forward(100);
            var expextedPostion = new Vec2D(0, -100);
            turtle.Position.IsApproximatelyEqualTo(expextedPostion, 0.001).Should().BeTrue();
        }
    }
}
