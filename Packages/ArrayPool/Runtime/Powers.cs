using System;

namespace DELTation.Pools
{
    internal static class Powers
    {
        public static int GetNextOrEqualPowerOfTwo(int length, int upperBound)
        {
            for (var powerOfTwo = 2; powerOfTwo <= upperBound; powerOfTwo *= 2)
            {
                if (length == powerOfTwo) return length;
                if (length < powerOfTwo) return powerOfTwo;
            }
            
            throw new InvalidOperationException("Matching power of two was not found.");
        }
    }
}