namespace NetDream.Shared.Interfaces
{
    public interface IFilterGroup
    {
        public string Name { get; }
        public string Label { get; }

        public bool Multiple { get; }

        public IFilterOption[] Items { get; }
    }

    public interface IFilterOption
    {
        public string Label { get; }
        public string Value { get; }
        public bool Selected { get; }
        public int Count { get; }
    }
}
