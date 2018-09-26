using System;
using System.Collections.Concurrent;

namespace Optimizations
{
    public static class Utilities
    {
        public static Func<Arg, Ret> Memoize<Arg, Ret>(this Func<Arg, Ret> functor)
        {
            var cache = new ConcurrentDictionary<Arg, Ret>();       // memoization cache
            var syncMap = new ConcurrentDictionary<Arg, object>();  // sync map for locking
            
            return currentArgument =>
            {
                if (!cache.TryGetValue(currentArgument, out Ret returnValue))
                {
                    var sync = syncMap.GetOrAdd(currentArgument, new object());
                    lock (sync)
                    {
                        returnValue = cache.GetOrAdd(currentArgument, functor);
                    }
                    syncMap.TryRemove(currentArgument, out sync);
                }
                return returnValue;
            };
        }
    }
}
