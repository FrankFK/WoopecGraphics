using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;

namespace TurtleCore.UnitTests
{
    [TestClass]
    public class ColorTest
    {

        [TestMethod]
        public void Color_CreateByPredefinedColor()
        {
            // Act
            var color = Colors.Green;

            // Assert
            color.R.Should().Be(0);
            color.G.Should().Be(128);
            color.B.Should().Be(0);
        }

        [TestMethod]
        public void Color_CreateByExistingName()
        {
            var pen = new Pen();
            pen.Color = "green";

            // Assert
            pen.Color.R.Should().Be(0);
            pen.Color.G.Should().Be(128);
            pen.Color.B.Should().Be(0);
        }

        [TestMethod]
        public void Color_CreateByNotExistingNameYieldsBlack()
        {
            var pen = new Pen();
            pen.Color = "unknownColor";

            // Assert
            pen.Color.Should().Be(Colors.Black);
        }

    }
}
