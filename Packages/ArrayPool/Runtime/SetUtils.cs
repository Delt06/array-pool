using System;
using System.Collections.Generic;

namespace DELTation.Pools
{
    internal static class SetUtils
    {
        public static T GetAny<T>(this HashSet<T> set)
        {
            if (set == null) throw new ArgumentNullException(nameof(set));

            foreach (var element in set)
            {
                return element;
            }
            
            throw new InvalidOperationException("Set contains no elements.");
        }
    }
}