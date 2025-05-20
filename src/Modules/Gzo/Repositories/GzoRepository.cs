using MySqlConnector;
using NetDream.Modules.Gzo.Entities;
using NetDream.Modules.Gzo.Storage;
using NetDream.Modules.Gzo.Templates;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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


        public TableEntity[] AllTableNames()
        {
            using var command = db.CreateCommand();
            command.CommandText = "select TABLE_NAME, TABLE_COMMENT from INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA=@0";
            command.Parameters.Add(new MySqlParameter("@0", Schema));
            using var reader = command.ExecuteReader();
            var data = new List<TableEntity>();
            while (reader.Read())
            {
                data.Add(new TableEntity()
                {
                    Name = reader.GetString(0),
                    Comment = reader.GetString(1)
                });
            }
            return [..data];
        }

        public ColumnEntity[] GetColumns(string table)
        {
            using var command = db.CreateCommand();
            command.CommandText = "select COLUMN_NAME, DATA_TYPE, COLUMN_KEY, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, COLUMN_DEFAULT, COLUMN_COMMENT from INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA=@1 AND TABLE_NAME=@0 order by ORDINAL_POSITION asc";
            command.Parameters.Add(new MySqlParameter("@0", table));
            command.Parameters.Add(new MySqlParameter("@1", Schema));
            using var reader = command.ExecuteReader();
            var data = new List<ColumnEntity>();
            while (reader.Read())
            {
                var type = reader.GetString(1);
                var key = reader.GetString(2);
                data.Add(new()
                {
                    Name = reader.GetString(0),
                    Type = type,
                    Length = !reader.IsDBNull(3) ? reader.GetInt32(3) : (!reader.IsDBNull(4) ? reader.GetInt32(4) : 0),
                    Default = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    Comment = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    IsPrimaryKey = key == "PRI",
                    IsUnique = key == "UNI"
                });
            }
            return [.. data];
        }

        public void BatchGenerateModel(string folder)
        {
            
        }

        public void BatchGenerateModel(string prefix, string folder, string module)
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                BatchGenerateModel(folder, module, AllTableNames());
                return;
            }
            BatchGenerateModel(
                folder, module,
                AllTableNames()
                .Where(table => table.Name.StartsWith(prefix)).ToArray());
        }

        public void BatchGenerateModel(string folder, string module, TableEntity[] tables)
        {
            var storage = new LocalStorage();
            storage.CreateFolder(folder);
            var entityFolder = Path.Combine(folder, "Entities");
            var migrationFolder = Path.Combine(folder, "Migrations");
            storage.CreateFolder(entityFolder);
            storage.CreateFolder(migrationFolder);
            var fileName = Path.Combine(folder, Template.ContextFileName(module));
            if (!storage.Exists(fileName))
            {
                using var writer = storage.Create(fileName);
                Template.Context(writer, module, tables);
            }
            foreach (var item in tables)
            {
                var columns = GetColumns(item.Name);
                fileName = Path.Combine(entityFolder, Template.EntityFileName(item));
                if (!storage.Exists(fileName))
                {
                    using var writer = storage.Create(fileName);
                    Template.Entity(writer, module, item, columns);
                }
                fileName = Path.Combine(migrationFolder, Template.MigrationFileName(item));
                if (!storage.Exists(fileName))
                {
                    using var writer = storage.Create(fileName);
                    Template.Migration(writer, module, item, columns);
                }
            }
            
        }
    }
}
