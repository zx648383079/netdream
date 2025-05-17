using NetDream.Modules.Gzo.Entities;
using NetDream.Shared.Template;

namespace NetDream.Modules.Gzo.Templates
{
    internal static partial class Template
    {
        public static string ContextFileName(string module)
        {
            return $"{module}Context.cs";
        }

        public static void Context(ICodeWriter writer, string module, TableEntity[] tables)
        {
            writer.WriteFormat("namespace NetDream.Modules.{0};", module)
                .WriteLine(true)
                .WriteFormat("public class {0}Context(DbContextOptions<{0}Context> options) : DbContext(options)", module)
                .WriteLine(true)
                .Write('{')
                .WriteIndentLine();

            foreach (var table in tables)
            {
                writer.WriteFormat("public DbSet<{0}Entity> {0} {{ get; set; }}", FormatTableName(table))
                    .WriteLine(true);
            }

            writer.Write("protected override void OnModelCreating(ModelBuilder modelBuilder)")
                .WriteLine(true)
                .Write('{')
                .WriteIndentLine();

            foreach (var table in tables)
            {
                writer.WriteFormat("modelBuilder.ApplyConfiguration(new {0}EntityTypeConfiguration());", FormatTableName(table))
                    .Write(true);
            }


            writer.Write("base.OnModelCreating(modelBuilder);")
                .WriteOutdentLine()
                .Write('}')

                .WriteOutdentLine()
                .Write('}');

        }
    }
}
