using NetDream.Shared.Helpers;
using NetDream.Modules.Gzo.Entities;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Data.Common;

namespace NetDream.Modules.Gzo.Repositories
{
    public partial class GzoRepository(DbConnection db)
    {
        [GeneratedRegex("Database=([\\w_]+?);")]
        private static partial Regex SchemaRegex();

        private string _schema = "zodream";

        public string Schema {
            get { return _schema; }
            set {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }
                if (!value.Contains('='))
                {
                    _schema = value;
                    return;
                }
                var match = SchemaRegex().Match(value);
                if (match.Success)
                {
                    _schema = match.Groups[1].Value;
                }
            }
        }


        public string[] AllTableNames()
        {
            using var command = db.CreateCommand();
            command.CommandText = "select TABLE_NAME as name from INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA=@0";
            command.Parameters.Add(Schema);
            using var reader = command.ExecuteReader();
            var data = new List<string>();
            while (reader.Read())
            {
                data.Add(reader.GetString(0));
            }
            return [..data];
        }

        public List<ColumnEntity> GetColumns(string table)
        {
            using var command = db.CreateCommand();
            command.CommandText = "select COLUMN_NAME as name, DATA_TYPE as type from INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA=@1 AND TABLE_NAME=@0 order by ORDINAL_POSITION asc";
            command.Parameters.Add(table);
            command.Parameters.Add(Schema);
            using var reader = command.ExecuteReader();
            var data = new List<ColumnEntity>();
            while (reader.Read())
            {
                data.Add(new()
                {
                    Name = reader.GetString(0),
                    Type = reader.GetString(1)
                });
            }
            return data;
        }

        public string Generate(string table)
        {
            return Generate(table, GetColumns(table));
        }

        public static string Generate(string table, List<ColumnEntity> columns)
        {
            var str = new StringBuilder();
            str.Append($"public class {FormatTableName(table)}Entity{FormatImplement(columns)}\n");
            str.Append("{\n");
            foreach (var item in columns)
            {
                str.AppendLine($"    public {FormatType(item)} {StrHelper.Studly(item.Name)} {{ get; set; }}{FormatDefaultValue(item)}\n");
            }
            str.Append('}');
            return str.ToString();
        }

        private static string FormatImplement(List<ColumnEntity> items)
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

        public static bool GenerateFile(string folder, string table, List<ColumnEntity> columns)
        {
            var index = table.IndexOf('_');
            var name = StrHelper.Studly(index >= 0 ? table[(index + 1)..] : table);
            var fileName = Path.Combine(folder, name + "Entity.cs");
            if (File.Exists(fileName))
            {
                return false;
            }
            var moduleName = index >= 0 ? StrHelper.Studly(table[..index]) : name;
            var str = new StringBuilder();
            str.AppendLine($"namespace {FormatNamespace(folder, moduleName)}");
            str.AppendLine("{");
            str.AppendLine($"    public class {name}Entity");
            str.AppendLine("    {");
            foreach (var item in columns)
            {
                str.AppendLine($"        public {FormatType(item)} {StrHelper.Studly(item.Name)} {{ get; set; }}{FormatDefaultValue(item)}");
            }
            str.AppendLine("    }");
            str.AppendLine("}");
            File.WriteAllText(fileName, str.ToString());
            return true;
        }

        public static string FormatNamespace(string folder, string def)
        {
            var i = folder.LastIndexOf("NetDream.Modules");
            if (i >= 0)
            {
                return folder[i..].Replace('/', '.').Replace('\\', '.');
            }
            i = folder.LastIndexOf("src");
            if (i >= 0)
            {
                return folder[(i + 4)..].Replace('/', '.').Replace('\\', '.');
            }
            return $"NetDream.Modules.{def}.Entities";
        }

        public static string FormatTableName(string table)
        {
            var i = table.IndexOf('_');
            return i >= 0 ? StrHelper.Studly(table[(i + 1)..]) : StrHelper.Studly(table);
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

        public void BatchGenerateModel(string folder)
        {
            BatchGenerateModel(AllTableNames(), folder);
        }

        public void BatchGenerateModel(string prefix, string folder)
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                BatchGenerateModel(folder);
                return;
            }
            BatchGenerateModel(AllTableNames().Where(name => name.StartsWith(prefix)), folder);
        }

        public void BatchGenerateModel(IEnumerable<string> tables, string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            foreach (var item in tables)
            {
                GenerateFile(folder, item, GetColumns(item));
            }
        }
    }
}
