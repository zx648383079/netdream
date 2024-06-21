using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Shared.Migrations
{
    public class OptionConfigureItem(string code, string name) : IOptionConfigureItem
    {
        public string Name { get; set; } = name;
        public string Code { get; set; } = code;
        public string Type { get; set; } = string.Empty;
        public int Visibility { get; set; }
        public string DefaultValue { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public int Position { get; set; }

        public OptionConfigureItem(string code, string name, string type, string defaultValue)
            : this(code, name)
        {
            Type = type;
            DefaultValue = defaultValue;
        }

        public OptionConfigureItem(
            string code, string name, string type, string defaultValue, int visibility)
            : this (code, name, type, defaultValue)
        {
            Visibility = visibility;
        }

        public OptionConfigureItem(string code, string name, int visibility)
            : this(code, name)
        {
            Visibility = visibility;
        }

        public OptionConfigureItem(string code, string name, string type, int visibility)
            : this(code, name)
        {
            Type = Type;
            Visibility = visibility;
        }
    }
}
