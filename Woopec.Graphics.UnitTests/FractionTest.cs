using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Woopec.Core.Examples;

namespace TurtleCore.UnitTests
{
    [TestClass]
    public class FractionTest
    {
        [TestMethod]
        public void GCD_Of_48_and_180()
        {
            Fractions.ResetPrimes();

            var gcd = Fractions.GCD(48, 180);

            gcd.Should().Be(12);
        }

        [TestMethod]
        public void GCD_Of_Two_Primes()
        {
            Fractions.ResetPrimes();

            var gcd = Fractions.GCD(89, 97);

            gcd.Should().Be(1);
        }

    }
}
