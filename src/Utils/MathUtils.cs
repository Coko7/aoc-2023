using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc_2023.src.Utils
{
    public class MathUtils
    {
        // Long functions

        public static long Gcd(long a, long b) => b == 0 ? a : Gcd(b, a % b);

        public static long Lcm(long a, long b) => Math.Abs(a * b) / Gcd(a, b);

        public static long Gcd(IEnumerable<long> numbers) => numbers.Aggregate(Gcd);

        public static long Lcm(IEnumerable<long> numbers) => numbers.Aggregate(Lcm);

        // Int functions

        public static int Gcd(int a, int b) => b == 0 ? a : Gcd(b, a % b);

        public static int Lcm(int a, int b) => Math.Abs(a * b) / Gcd(a, b);

        public static int Gcd(IEnumerable<int> numbers) => numbers.Aggregate(Gcd);

        public static int Lcm(IEnumerable<int> numbers) => numbers.Aggregate(Lcm);
    }
}
