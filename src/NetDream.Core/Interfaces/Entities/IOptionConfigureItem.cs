namespace NetDream.Core.Interfaces.Entities
{
    public interface IOptionConfigureItem
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public int Visibility { get; set; }
        public string DefaultValue { get; set; }
        public string Value { get; set; }
        public int Position { get; set; }
    }
}
