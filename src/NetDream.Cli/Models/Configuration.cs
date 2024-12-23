namespace NetDream.Cli.Models
{
    public class Configuration
    {
        public ConnectionString ConnectionStrings { get; set; } = new();
    }

    public class ConnectionString
    {
        public string Default { get; set; } = string.Empty;
    }
}
