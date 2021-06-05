using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;

namespace Woopec.Core.UnitTests
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
            Color color = "green";

            // Assert
            color.R.Should().Be(0);
            color.G.Should().Be(128);
            color.B.Should().Be(0);
        }

        [TestMethod]
        public void Color_CreateByNotExistingNameYieldsBlack()
        {
            Color color = "unknownColor";

            // Assert
            color.Should().Be(Colors.Black);
        }

    }
}
