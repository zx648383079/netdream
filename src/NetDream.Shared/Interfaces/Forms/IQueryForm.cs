namespace NetDream.Shared.Interfaces
{
    public interface IQueryForm: IPaginationForm
    {
        public string? Keywords { get; }

        public string? Sort { get; }

        public string? Order { get; }
    }
}
