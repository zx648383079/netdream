namespace NetDream.Shared.Interfaces
{
    public interface IPagination
    {
        public int ItemsPerPage { get; }
        public int CurrentPage { get; }
        public int TotalItems { get; }
        public int TotalPages { get; }
    }
    public interface IPage<T> : IPagination
    {
        public T[] Items { get; }
    }

 
}
