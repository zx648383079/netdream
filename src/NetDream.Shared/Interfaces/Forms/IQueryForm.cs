namespace NetDream.Shared.Interfaces.Forms
{
    public interface IQueryForm: IPaginationForm
    {
        public string? Keywords { get; }

        public string? Sort { get; }

        public string? Order { get; }
    }
}
