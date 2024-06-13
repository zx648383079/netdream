using NetDream.Core.Helpers;
using NetDream.Modules.Gzo.Entities;
using NPoco;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace NetDream.Modules.Gzo.Repositories
{
    public partial class GzoRepository
    {
        [GeneratedRegex("Database=([\\w_]+?);")]
        private static partial Regex SchemaRegex();

        private readonly IDatabase _db;

        public GzoRepository(IDatabase db)
        {
            _db = db;
        }

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
            var data = _db.Fetch<TableEntity>("select TABLE_NAME as name from INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA=@0", Schema);
            var tables = new string[data.Count];
            var i = 0;
            foreach (var item in data)
            {
                tables[i++] = item.Name;
            }
            return tables;
        }

        public List<ColumnEntity> GetColumns(string table)
        {
            return _db.Fetch<ColumnEntity>("select COLUMN_NAME as name, DATA_TYPE as type from INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA=@1 AND TABLE_NAME=@0 order by ORDINAL_POSITION asc", table, Schema);
        }

        public string Generate(string table)
        {
            return Generate(table, GetColumns(table));
        }

        public static string Generate(string table, List<ColumnEntity> columns)
        {
            var str = new StringBuilder();
            str.Append("[TableName(ND_TABLE_NAME)]\n");
            str.Append($"public class {FormatTableName(table)}Entity\n");
            str.Append("{\n");
            str.Append($"    internal const string ND_TABLE_NAME = \"{table}\";\n");
            foreach (var item in columns)
            {
                if (item.Name.IndexOf('_') > 0)
                {
                    str.Append($"    [Column(\"{item.Name}\")]\n");
                }
                str.AppendLine($"    public {FormatType(item)} {StrHelper.Studly(item.Name)} {{ get; set; }}{FormatDefaultValue(item)}\n");
            }
            str.Append("}");
            return str.ToString();
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
            str.AppendLine("using NPoco;");
            str.AppendLine($"namespace {FormatNamespace(folder, moduleName)}");
            str.AppendLine("{");
            str.AppendLine("    [TableName(ND_TABLE_NAME)]");
            str.AppendLine($"    public class {name}Entity");
            str.AppendLine("    {");
            str.AppendLine($"        internal const string ND_TABLE_NAME = \"{table}\";");
            foreach (var item in columns)
            {
                if (item.Name.IndexOf('_') > 0)
                {
                    str.AppendLine($"        [Column(\"{item.Name}\")]");
                }
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
            foreach (var item in tables)
            {
                GenerateFile(folder, item, GetColumns(item));
            }
        }
    }
}
