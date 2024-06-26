namespace NetDream.Modules.SEO.Models
{
    public class OptionModel
    {
        public string Code { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;

        public object? FormatValue()
        {
            return Type switch
            {
                "switch" => Value == "1" || Value == "true",
                "json" => string.IsNullOrWhiteSpace(Value) ? null : "",
                _ => Value,
            };
        }
    }
}
