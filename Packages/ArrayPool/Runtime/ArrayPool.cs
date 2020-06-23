namespace DELTation.Pools
{
    public static class ArrayPool<T>
    {
        public static T[] Rent(int minLength) => Instance.Rent(minLength);
        public static bool InRent(T[] array) => Instance.InRent(array);
        public static void Return(T[] array) => Instance.Return(array);

        public const int MaxSupportedLength = 1 << 24;

        public static IArrayPool<T> Instance { get; } = new StandaloneArrayPool<T>(MaxSupportedLength);
    }
}