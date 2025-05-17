using NetDream.Modules.Gzo.Entities;
using NetDream.Shared.Helpers;
using NetDream.Shared.Template;
using System.Linq;

namespace NetDream.Modules.Gzo.Templates
{
    internal static partial class Template
    {
        public static string MigrationFileName(TableEntity table)
        {
            return $"{FormatTableName(table)}EntityTypeConfiguration.cs";
        }

        public static void Migration(ICodeWriter writer, string module, TableEntity table, ColumnEntity[] columns)
        {
            writer.WriteFormat("namespace NetDream.Modules.{0}.Migrations;", module)
                .WriteLine(true)
                .WriteFormat("public class {0}EntityTypeConfiguration : IEntityTypeConfiguration<{0}Entity>", FormatTableName(table))
                .WriteLine(true)
                .Write('{')
                .WriteIndentLine()

                .WriteFormat("public void Configure(EntityTypeBuilder<{0}Entity> builder)", FormatTableName(table))
                .WriteLine(true)
                .Write('{')
                .WriteIndentLine()

                .WriteFormat("builder.ToTable(\"{0}\", table => table.HasComment(\"{1}\"));", table.Name, table.Comment)
                .WriteLine(true);
            if (columns.Where(i => i.Name.Equals("id", System.StringComparison.OrdinalIgnoreCase)).Any())
            {
                writer.WriteFormat("builder.HasKey(table => table.Id);")
                    .WriteLine(true);
            }
            foreach (var column in columns)
            {
                writer.WriteFormat("builder.Property(table => table.{0}).HasColumnName(\"{1}\")", StrHelper.Studly(column.Name), column.Name);
                if (column.Length > 0)
                {
                    writer.WriteFormat(".HasMaxLength({0})", column.Length);
                }
                if (column.Default is not null)
                {
                    writer.WriteFormat(".HasDefaultValue({0})", column.Default);
                }
                if (!string.IsNullOrWhiteSpace(column.Comment))
                {
                    writer.WriteFormat(".HasComment(\"{0}\")", column.Comment);
                }
                writer.Write(';').WriteLine(true);
            }

            writer.WriteOutdentLine()
                .Write('}')

                .WriteOutdentLine()
                .Write('}');

        }
    }
}
