using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectCreation;

namespace FastObjectAllocatorTest
{
    [TestClass]
    public class PerformanceTest
    {
        static object CreateNativeObject() => new object();
        static T CreateDynamicObject<T>() where T : new() => new T();
        static object CreateReflectiveObject(Type objectType) => Activator.CreateInstance(objectType);
        static T CreateFastObject<T>() where T : new() => FastObjectAllocator<T>.New();

        /// <summary>
        /// The Measure function will loop through and run a given function many times
        /// in order to calculate basic statistics on its performance
        /// </summary>
        /// <param name="message">A custom output message for the job</param>
        /// <param name="sets">The outer number of loops to perform (Each set is aggregated separately from each other)</param>
        /// <param name="repititions">The number of times to perform the action within a set</param>
        /// <param name="action">The function to run</param>
        /// <returns></returns>
        private static void Measure(string message, int sets, int repititions, Action action)
        {
            // clean-up
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // warm-up
            action();

            double[] results = new double[sets];
            for (int i = 0; i < sets; i++)
            {
                Stopwatch timer = Stopwatch.StartNew();

                for(int j = 0; j < repititions; j++)
                    action();

                results[i] = timer.Elapsed.TotalMilliseconds;
            }

            Console.WriteLine($"{ message }: " +
                $"Average = { results.Average() }, " +
                $"Min = { results.Min() }, " +
                $"Max = { results.Max() }");
        }

        [TestMethod]
        public void TestObjectCreation()
        {
            int sets = 5;
            int repititions = 100000;
            
            Measure("Create Native Objects", sets, repititions, () => { CreateNativeObject(); });
            Measure("Create Dynamic Objects", sets, repititions, () => { CreateDynamicObject<object>(); });
            Measure("Create Reflective Objects", sets, repititions, () => { CreateReflectiveObject(typeof(Object)); });
            Measure("Create Fast Objects", sets, repititions, () => { CreateFastObject<object>(); });
        }
    }
}
