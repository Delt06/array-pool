namespace DELTation.Pools
{
    public interface IArrayPool<T>
    {
        int MaxSupportedLength { get; }
        T[] Rent(int minLength);
        bool InRent(T[] array);
        void Return(T[] array);
    }
}