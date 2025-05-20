using NetDream.Modules.Gzo.Entities;
using NetDream.Shared.Helpers;
using NetDream.Shared.Template;
using System;
using System.Collections.Generic;

namespace NetDream.Modules.Gzo.Templates
{
    internal static partial class Template
    {

        public static string EntityFileName(TableEntity table)
        {
            return $"{FormatTableName(table)}Entity.cs";
        }

        public static void Entity(ICodeWriter writer, string module, TableEntity table, ColumnEntity[] columns)
        {
            var impl = FormatImplement(columns);
            if (!string.IsNullOrWhiteSpace(impl))
            {
                writer.Write("using NetDream.Shared.Interfaces.Entities;")
                .WriteLine(true)
                .WriteLine(true);
            }
            writer.WriteFormat("namespace NetDream.Modules.{0}.Entities;", module)
                .WriteLine(true)
                .WriteFormat("public class {0}Entity{1}", FormatTableName(table), impl)
                .WriteLine(true)
                .Write('{')
                .WriteIndentLine();
            foreach (var column in columns)
            {
                writer.WriteFormat("public {1} {0} {{ get; set; }}{2}", 
                    StrHelper.Studly(column.Name), FormatType(column), FormatDefaultValue(column))
                    .WriteLine(true);
            }
            writer.WriteOutdentLine()
                .Write('}');

        }

        public static string FormatType(ColumnEntity data)
        {
            return data.Type switch
            {
                "tinyint" => "byte",
                "smallint" => "int",
                "char" or "varchar" or "text" or "mediumtext" or "longtext" or "enum" => "string",
                "date" => "string",
                "time" => "TimeSpan",
                "datetime" => "DateTime",
                "decimal" => "float",
                _ => data.Type,
            };
        }

        public static string FormatDefaultValue(ColumnEntity data)
        {
            return data.Type switch
            {
                "char" or "varchar" or "text" or "mediumtext" or "longtext" or "enum" or "date" => " = string.Empty;",
                _ => "",
            };
        }

        private static string FormatTableName(TableEntity table)
        {
            var index = table.Name.IndexOf('_');
            return StrHelper.Studly(index >= 0 ? table.Name[(index + 1)..] : table.Name);
        }

        private static string FormatImplement(ColumnEntity[] items)
        {
            var hasId = false;
            var hasCreated = false;
            var hasUpdated = false;
            foreach (var item in items)
            {
                if (item.Name.Equals("id", StringComparison.OrdinalIgnoreCase))
                {
                    hasId = true;
                }
                else if (item.Name.Equals("created_at", StringComparison.OrdinalIgnoreCase))
                {
                    hasCreated = true;
                }
                else if (item.Name.Equals("updated_at", StringComparison.OrdinalIgnoreCase))
                {
                    hasUpdated = true;
                }
            }
            var res = new List<string>();
            if (hasId)
            {
                res.Add("IIdEntity");
            }
            if (hasCreated)
            {
                res.Add(hasUpdated ? "ITimestampEntity" : "ICreatedEntity");
            }
            if (res.Count == 0)
            {
                return string.Empty;
            }
            return " : " + string.Join(", ", res);
        }
    }
}
