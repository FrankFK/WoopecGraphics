using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;

namespace Woopec.Graphics.UnitTests
{
    [TestClass]
    public class SpeedTest
    {

        [TestMethod]
        public void Speed_CreateByPredefinedSpeed()
        {
            // Act
            Speed speed = Speeds.Normal;

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
            Speed speed = Speeds.Fastest;

            // Assert
            speed.NoAnimation.Should().BeTrue();
        }

        [TestMethod]
        public void Speed_TooLowResultsToNoAnimation()
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
            Speed speed = Speeds.Fast;

            // two non trivial vectors with distance 500
            var from = new Vec2D(100, 200);
            var to = new Vec2D(100 + 300, 200 + 400);

            int duration = speed.MillisecondsForMovement(from, to);
            duration.Should().Be(212);
        }

        [TestMethod]
        public void Speed_DurationForSlowestSpeed()
        {
            Speed speed = Speeds.Slowest;

            // two non trivial vectors with distance 500
            var from = new Vec2D(100, 200);
            var to = new Vec2D(100 + 300, 200 + 400);

            int duration = speed.MillisecondsForMovement(from, to);
            duration.Should().Be(5000);
        }

        [TestMethod]
        public void Speed_DurationForFullRotationOnSlowestSpeed()
        {
            Speed speed = Speeds.Slowest;

            int duration = speed.MillisecondsForRotation(0, 360);
            duration.Should().Be(2300);
        }

        [TestMethod]
        public void Speed_DurationForFullRotationOnFastSpeed()
        {
            Speed speed = Speeds.Fast;

            int duration = speed.MillisecondsForRotation(0, 360);
            duration.Should().Be(230);
        }
    }
}
