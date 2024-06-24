namespace NetDream.Shared.Repositories.Models
{
    public interface ILanguageFormatted
    {
        public int Id { get; }
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
