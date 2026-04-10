namespace NetDream.Shared.Interfaces
{
    public interface IQueryForm: IPaginationForm
    {
        public string? Keywords { get; }

        public string? Sort { get; }

        public string? Order { get; }
    }

    public interface ISourceQueryForm: IQueryForm
    {
        public int User { get; }

        public int Type { get; }
    }
}
