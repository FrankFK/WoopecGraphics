using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;

namespace TurtleCore.UnitTests
{
    [TestClass]
    public class SpeedTest
    {

        [TestMethod]
        public void Speed_CreateByPredefinedSpeed()
        {
            // Act
            Speed speed = SpeedLevel.Normal;

            // Assert
            speed.Value.Should().Be(6);
        }

        [TestMethod]
        public void Speed_CreateByValue()
        {
            // Act
            Speed speed = 6;

            // Assert
            speed.Value.Should().Be(6);
        }

        [TestMethod]
        public void Speed_FastestMeansNoAnimation()
        {
            // Act
            Speed speed = SpeedLevel.Fastest;

            // Assert
            speed.NoAnimation.Should().BeTrue();
        }

        [TestMethod]
        public void Speed_TooLowsResultsToNoAnimation()
        {
            // Act
            Speed speed = 0.4;

            // Assert
            speed.NoAnimation.Should().BeTrue();
        }

        /// <summary>
        /// In contrast to python there is no speed limit
        /// </summary>
        [TestMethod]
        public void Speed_NoSpeedLimit()
        {
            // Act
            Speed speed = 100;

            // Assert
            speed.NoAnimation.Should().BeFalse();
            speed.Value.Should().Be(100);
        }

        [TestMethod]
        public void Speed_DurationForFastSpeed()
        {
            Speed speed = SpeedLevel.Fast;

            // two non trivial vectors with distance 500
            var from = new Vec2D(100, 200);
            var to = new Vec2D(100 + 300, 200 + 400);

            int duration = speed.MillisecondsForMovement(from, to);
            duration.Should().Be(212);
        }

        [TestMethod]
        public void Speed_DurationForSlowestSpeed()
        {
            Speed speed = SpeedLevel.Slowest;

            // two non trivial vectors with distance 500
            var from = new Vec2D(100, 200);
            var to = new Vec2D(100 + 300, 200 + 400);

            int duration = speed.MillisecondsForMovement(from, to);
            duration.Should().Be(5000);
        }
    }
}
