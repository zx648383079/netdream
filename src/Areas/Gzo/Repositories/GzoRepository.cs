using Microsoft.AspNetCore.Identity;
using NetDream.Areas.Auth.Models;
using NetDream.Areas.Gzo.Models;
using NetDream.Base.Helpers;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Areas.Gzo.Repositories
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
            var data = _db.Fetch<TableItem>("select TABLE_NAME as name from INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='zodream'");
            var tables = new string[data.Count];
            var i = 0;
            foreach (var item in data)
            {
                tables[i++] = item.Name;
            }
            return tables;
        }

        public List<ColumnItem> GetColumns(string table)
        {
            return _db.Fetch<ColumnItem>("select COLUMN_NAME as name, DATA_TYPE as type from INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='zodream' AND TABLE_NAME=@0 order by ORDINAL_POSITION asc", table);
        }

        public string Generate(string table)
        {
            return Generate(table, GetColumns(table));
        }

        public string Generate(string table, List<ColumnItem> columns)
        {
            var str = new StringBuilder();
            str.Append("[TableName(\"");
            str.Append(table);
            str.Append("\")]");
            str.Append("\npublic class ");
            str.Append(Str.Studly(table));
            str.Append("\n{");
            foreach (var item in columns)
            {
                str.Append("\n    public ");
                str.Append(item.ToType());
                str.Append(" ");
                str.Append(Str.Studly(item.Name));
                str.Append(" { get; set; }");
            }
            str.Append("\n}");
            return str.ToString();
        }
    }
}
