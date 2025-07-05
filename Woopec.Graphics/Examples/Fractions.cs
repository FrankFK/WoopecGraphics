using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Core.Examples
{
    internal class Fractions
    {
        private static readonly List<int> s_primes = new() { 2 };

        /// <summary>
        /// Calculate the lowest common multiple of two numbers
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>lowestt common multiple</returns>
        public static int LCM(int a, int b)
        {
            return Math.Abs(a * b) / GCD(a, b);
        }

        /// <summary>
        /// Calculate the greatest common divisor of two numbers
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int GCD(int a, int b)
        {
            // I'm using prime factorizations here. I know that e.g. the Euclidea algorithm is faster.
            // But prime factorization is easier to understand.

            var gcd = 1;
            var lowerValue = Math.Min(a, b);
            var higherValue = Math.Max(a, b);

            var factorsOfLowerValue = PrimeFactorsOf(lowerValue);
            var factorsOfHigherValue = PrimeFactorsOf(higherValue);

            foreach (var factorOfLowerValue in factorsOfLowerValue)
            {
                var prime = factorOfLowerValue.Key;

                var countInHigher = factorsOfHigherValue.GetValueOrDefault(prime);
                if (countInHigher >= 0)
                {
                    var countInLower = factorOfLowerValue.Value;
                    gcd *= (int)Math.Pow(prime, Math.Min(countInLower, countInHigher));
                }
            }

            return gcd;
        }



        /// <summary>
        /// Calculate the prime factors of a number
        /// </summary>
        /// <param name="n"></param>
        /// <returns>All prime factors as a dictionary of tuples (prime, count).</returns>
        public static Dictionary<int, int> PrimeFactorsOf(int n)
        {
            var primeFactors = new Dictionary<int, int>();

            var highestPossibleDivisor = Math.Sqrt(n);
            UpdatePrimesUpTo(highestPossibleDivisor);

            // Divide n by all prime numbers smaller than sqrt(n):
            var primeIndex = 0;
            var rest = n;
            while (primeIndex < s_primes.Count() && s_primes[primeIndex] < highestPossibleDivisor && rest != 1)
            {
                var prime = s_primes[primeIndex];

                var count = 0; // how often can n be divided by prime
                while (rest % prime == 0)
                {
                    rest /= prime;
                    count++;
                }

                if (count > 0)
                    primeFactors.Add(prime, count);

                primeIndex++;
            }

            // This case happens if n is a prime number
            if (rest > 1)
                primeFactors[rest] = 1;

            return primeFactors;
        }

        /// <summary>
        /// Only for unit-tests
        /// </summary>
        internal static void ResetPrimes()
        {
            s_primes.Clear();
            s_primes.Add(2);
        }

        private static void UpdatePrimesUpTo(double n)
        {
            var lastKnownPrime = s_primes.Last();
            for (var i = lastKnownPrime + 1; i <= n; i++)
            {
                if (!s_primes.Exists(p => i % p == 0))
                    s_primes.Add(i);
            }
        }

        private static int LowestCommonMultipleOfWithoutGcd(int a, int b)
        {
            var lcm = 1;
            var lowerValue = Math.Min(a, b);
            var higherValue = Math.Max(a, b);

            var factorsOfHigherValue = PrimeFactorsOf(higherValue);
            var factorsOfLowerValue = PrimeFactorsOf(lowerValue);

            foreach (var factorOfLowerValue in factorsOfLowerValue)
            {
                var prime = factorOfLowerValue.Key;
                var countInLower = factorOfLowerValue.Value;

                var countInHiger = factorsOfHigherValue.GetValueOrDefault(prime);
                if (countInHiger >= 0)
                {
                    var maxCount = Math.Max(countInLower, countInHiger);
                    lcm *= (int)Math.Pow(prime, maxCount);
                    factorsOfHigherValue.Remove(prime);
                }
                else
                {
                    lcm *= (int)Math.Pow(prime, countInLower);
                }
            }

            foreach (var primeFactorOfHigher in factorsOfHigherValue)
            {
                lcm *= (int)Math.Pow(primeFactorOfHigher.Key, primeFactorOfHigher.Value);
            }

            return lcm;
        }

    }
}
