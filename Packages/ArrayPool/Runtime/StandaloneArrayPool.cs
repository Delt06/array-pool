using System;
using System.Collections.Generic;

namespace DELTation.Pools
{
    public sealed class StandaloneArrayPool<T> : IArrayPool<T>
    {
        public int MaxSupportedLength { get; }

        public StandaloneArrayPool(int maxSupportedLength = int.MaxValue)
        {
            if (maxSupportedLength <= 0) throw new ArgumentOutOfRangeException(nameof(maxSupportedLength));
            
            MaxSupportedLength = maxSupportedLength;
        }
        
        public T[] Rent(int minLength)
        {
            if (minLength <= 0) throw new ArgumentOutOfRangeException(nameof(minLength));
            if (minLength > MaxSupportedLength) throw new ArgumentOutOfRangeException(nameof(minLength));

            var powerOfTwoLength = Powers.GetNextOrEqualPowerOfTwo(minLength, MaxSupportedLength);
            var rentedArray = GetOrCreateFreeArray(powerOfTwoLength);
            MarkAsRented(rentedArray);

            return rentedArray;
        }

        private T[] GetOrCreateFreeArray(int length)
        {
            var freeSet = GetOrCreateFreeSet(length);
            return freeSet.Count > 0 ? freeSet.GetAny() : new T[length];
        }

        private void MarkAsRented(T[] array)
        {
            var freeSet = GetOrCreateFreeSet(array.Length);
            freeSet.Remove(array);
            _rentedArrays.Add(array);
        }

        private HashSet<T[]> GetOrCreateFreeSet(int length)
        {
            if (_freeArrays.TryGetValue(length, out var freeSet)) return freeSet;
            
            freeSet = new HashSet<T[]>();

            for (var index = 0; index < InitialArrayCount; index++)
            {
                freeSet.Add(new T[length]);
            }

            _freeArrays[length] = freeSet;

            return freeSet;
        }

        public bool InRent(T[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            return _rentedArrays.Contains(array);
        }

        public void Return(T[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (!_rentedArrays.Contains(array)) return;

            _rentedArrays.Remove(array);
            var freeSet = GetOrCreateFreeSet(array.Length);
            freeSet.Add(array);
        }

        private readonly IDictionary<int, HashSet<T[]>> _freeArrays = new Dictionary<int, HashSet<T[]>>();
        private readonly ISet<T[]> _rentedArrays = new HashSet<T[]>();

        private const int InitialArrayCount = 16;
    }
}