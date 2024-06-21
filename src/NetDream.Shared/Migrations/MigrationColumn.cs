using NetDream.Shared.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Shared.Migrations
{
    public class MigrationColumn : ITableColumn
    {
        public ITableColumn Unique()
        {
            return this;
        }
        public ITableColumn Default(object val)
        {
            return this;
        }

        public ITableColumn Comment(string comment)
        {
            return this;
        }

        public ITableColumn Nullable()
        {
            return this;
        }

        public ITableColumn Nullable(bool nullable)
        {
            return this;
        }

        public ITableColumn Ai(int begin)
        {
            return this;
        }
    }
}
