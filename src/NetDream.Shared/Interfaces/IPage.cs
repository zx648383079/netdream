namespace NetDream.Shared.Interfaces
{
    public interface IPage<T>
    {
        public T[] Items { get; }

        public int ItemsPerPage { get; }
        public int CurrentPage { get; }
        public int TotalItems { get; }
        public int TotalPages { get; }
    }
}
