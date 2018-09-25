using System;
using System.Collections.Concurrent;

namespace Optimizations
{
    public static class Utilities
    {
        public static Func<Arg, Ret> Memoize<Arg, Ret>(this Func<Arg, Ret> functor)
        {
            var cache = new ConcurrentDictionary<Arg, Ret>();

            return argument => cache.GetOrAdd(argument, functor);
        }
    }
}
