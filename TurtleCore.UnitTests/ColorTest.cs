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

        [TestMethod]
        public void Color_FromHSV_Red() { Color.FromHSV(0, 1.0, 1.0).Should().Be(Colors.Red); }

        [TestMethod]
        public void Color_FromHSV_Blue() { Color.FromHSV(240, 1.0, 1.0).Should().Be(Colors.Blue); }

        [TestMethod]
        public void Color_FromHSV_Violet() { Color.FromHSV(270, 1.0, 1.0).Should().Be(new Color(127, 0, 255)); }

        [TestMethod]
        public void Color_FromHSV_ValueNull() { Color.FromHSV(270, 0.8, 0.0).Should().Be(Colors.Black); }

        [TestMethod]
        public void Color_FromHSV_SaturationNull() { Color.FromHSV(111, 0.0, 0.5).Should().Be(new Color(127, 127, 127)); }
    }
}
