namespace NetDream.Modules.Gzo.Entities
{
    public class ColumnEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;
        public int Length { get; set; }
        public object? Default { get; set; }
        public string Comment { get; set; } = string.Empty;

        public bool IsUnique { get; set; }
        public bool IsPrimaryKey { get; set; }
    }
}
