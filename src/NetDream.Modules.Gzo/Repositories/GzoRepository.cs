using NetDream.Core.Helpers;
using NetDream.Modules.Gzo.Entities;
using NPoco;
using System.Text;

namespace NetDream.Modules.Gzo.Repositories
{
    public class GzoRepository
    {
        private readonly IDatabase _db;

        public GzoRepository(IDatabase db)
        {
            _db = db;
        }

        public string[] AllTableNames()
        {
            var data = _db.Fetch<TableEntity>("select TABLE_NAME as name from INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='zodream'");
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
            return _db.Fetch<ColumnEntity>("select COLUMN_NAME as name, DATA_TYPE as type from INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='zodream' AND TABLE_NAME=@0 order by ORDINAL_POSITION asc", table);
        }

        public string Generate(string table)
        {
            return Generate(table, GetColumns(table));
        }

        public static string Generate(string table, List<ColumnEntity> columns)
        {
            var str = new StringBuilder();
            str.Append("[TableName(\"");
            str.Append(table);
            str.Append("\")]");
            str.Append("\npublic class ");
            str.Append(FormatTableName(table));
            str.Append("Entity\n{");
            foreach (var item in columns)
            {
                if (item.Name.IndexOf('_') > 0)
                {
                    str.Append($"\n    [Column(\"{item.Name}\")]");
                }
                str.Append("\n    public ");
                str.Append(FormatType(item));
                str.Append(' ');
                str.Append(Str.Studly(item.Name));
                str.Append(" { get; set; }");
            }
            str.Append("\n}");
            return str.ToString();
        }

        public static bool GenerateFile(string folder, string table, List<ColumnEntity> columns)
        {
            var index = table.IndexOf('_');
            var name = Str.Studly(index >= 0 ? table.Substring(index + 1) : table);
            var fileName = Path.Combine(folder, name + "Entity.cs");
            if (File.Exists(fileName))
            {
                return false;
            }
            var moduleName = index >= 0 ? Str.Studly(table[..index]) : name;
            var str = new StringBuilder();
            str.AppendLine("using NPoco;");
            str.AppendLine($"namespace NetDream.Modules.{moduleName}.Entities");
            str.AppendLine("{");
            str.AppendLine($"    [TableName(\"{table}\")]");
            str.AppendLine($"    public class {name}Entity");
            str.AppendLine("    {");
            foreach (var item in columns)
            {
                if (item.Name.IndexOf('_') > 0)
                {
                    str.AppendLine($"        [Column(\"{item.Name}\")]");
                }
                str.AppendLine($"        public {FormatType(item)} {Str.Studly(item.Name)} {{ get; set; }}");
            }
            str.AppendLine("    }");
            str.AppendLine("}");
            File.WriteAllText(fileName, str.ToString());
            return true;
        }

        public static string FormatTableName(string table)
        {
            var i = table.IndexOf('_');
            return i >= 0 ? Str.Studly(table[(i + 1)..]) : Str.Studly(table);
        }

        public static string FormatType(ColumnEntity data)
        {
            return data.Type switch
            {
                "tinyint" or "smallint" => "int",
                "char" or "varchar" or "text" or "mediumtext" or "enum" => "string?",
                "date" => "string?",
                _ => data.Type,
            };
        }

        public void BatchGenerateModel(string folder)
        {
            BatchGenerateModel(AllTableNames(), folder);
        }

        public void BatchGenerateModel(string prefix, string folder)
        {
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
