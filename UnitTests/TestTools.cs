using System;
using System.Diagnostics;
using System.Linq;

namespace UnitTests
{
    public static class TestTools
    {
        /// <summary>
        /// The Measure function will loop through and run a given function many times
        /// in order to calculate basic statistics on its performance
        /// </summary>
        /// <param name="message">A custom output message for the job</param>
        /// <param name="measurements">The outer number of loops to perform</param>
        /// <param name="repititions">The number of times to perform the action within a measurement</param>
        /// <param name="action">The function to run</param>
        /// <returns>Returns an array of measurements (double)</returns>
        public static double[] Measure(string message, int measurements, int repititions, Action action)
        {
            // clean-up
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // warm-up
            action();

            double[] results = new double[measurements];
            for (int i = 0; i < measurements; i++)
            {
                Stopwatch timer = Stopwatch.StartNew();

                for (int j = 0; j < repititions; j++)
                    action();

                results[i] = timer.Elapsed.TotalMilliseconds;
            }

            Console.WriteLine($"{ message }: " +
                $"Average = { results.Average() }ms, " +
                $"Min = { results.Min() }ms, " +
                $"Max = { results.Max() }ms");

            return results;
        }
    }
}
