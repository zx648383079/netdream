namespace NetDream.Shared.Repositories.Models
{
    public interface ILanguageFormatted
    {
        public int Id { get; }
        public string Name { get; }

        public string Value { get; }
    }
}
