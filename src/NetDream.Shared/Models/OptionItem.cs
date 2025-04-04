namespace NetDream.Shared.Models
{
    public class OptionItem(string name, object value)
    {
        public string Name { get; set; } = name;

        public object Value { get; set; } = value;
    }

    public class OptionItem<T>(string name, T value)
    {
        public string Name { get; set; } = name;

        public T Value { get; set; } = value;
    }

    public class LinkOptionItem(string name, string label, string icon, string url)
    {
        public string Name { get; set; } = name;

        public string Label { get; set; } = label;
        public string Icon { get; set; } = icon;
        public string Url { get; set; } = url;

        public LinkOptionItem(string name, string label, string url)
            : this(name, label, string.Empty, url)
        {
            
        }
    }
}
