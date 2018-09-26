using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Optimizations;

namespace UnitTests
{
    [TestClass]
    public class Memoization
    {
        private static long Fibonacci(int n)
        {
            if (n > 1)
                return Fibonacci(n - 1) + Fibonacci(n - 2);
            else if (n == 1)
                return 1;
            else
                return 0;
        }

        private static long PrimeNumber(int n)
        {
            int count = 0;
            long a = 2;
            while (count < n)
            {
                long b = 2;
                int prime = 1;
                while (b * b <= a)
                {
                    if (a % b == 0)
                    {
                        prime = 0;
                        break;
                    }
                    b++;
                }
                if (prime > 0)
                    count++;
                a++;
            }
            return (--a);
        }

        [TestMethod]
        public void TestRecursionOptimization()
        {
            var MemoizedFibonacci = Utilities.Memoize<int, long>(Fibonacci);
            var MemoizedPrimeNumber = Utilities.Memoize<int, long>(PrimeNumber);

            int sets = 5;
            int repititions = 10000;

            TestTools.Measure("Fibonacci Sequence", sets, repititions, () => { Fibonacci(20); });
            TestTools.Measure("Memoized Fibonacci Sequence", sets, repititions, () => { MemoizedFibonacci(20); });
            TestTools.Measure("Prime Number", sets, repititions, () => { PrimeNumber(50); });
            TestTools.Measure("Memoized Prime Number", sets, repititions, () => { MemoizedPrimeNumber(50); });
        }
    }
}
