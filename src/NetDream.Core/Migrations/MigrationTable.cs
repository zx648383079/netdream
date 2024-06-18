using NetDream.Core.Interfaces.Database;
using System.Collections.Generic;

namespace NetDream.Core.Migrations
{
    public class MigrationTable: ITable
    {
        public const string COLUMN_CREATED_AT = "created_at";
        public const string COLUMN_UPDATED_AT = "updated_at";
        public const string COLUMN_DELETED_AT = "deleted_at";
        public ITableColumn Id(string column = "id")
        {

            return new MigrationColumn();
        }

        public ITableColumn Enum(string column, IEnumerable<string> items)
        {
            return new MigrationColumn();
        }
        public ITableColumn String(string column, int length = 255)
        {
            return new MigrationColumn();
        }

        public ITableColumn Time(string column)
        {
            return new MigrationColumn();
        }

        public ITableColumn Uint(string column, int length = 10)
        {
            return new MigrationColumn();
        }

        public ITableColumn Decimal(string column, int length = 16, int d = 10)
        {
            return new MigrationColumn();
        }
        public ITableColumn Float(string column, int length = 16, int d = 10)
        {
            return new MigrationColumn();
        }
        public ITableColumn Double(string column, int length = 16, int d = 10)
        {
            return new MigrationColumn();
        }

        public ITableColumn Char(string column, int length = 20)
        {
            return new MigrationColumn();
        }

        public ITableColumn Int(string column)
        {
            return new MigrationColumn();
        }

        public ITableColumn Date(string column)
        {
            return new MigrationColumn();
        }

        public ITableColumn Text(string column)
        {
            return new MigrationColumn();
        }

        public ITableColumn MediumText(string column)
        {
            return new MigrationColumn();
        }

        public ITableColumn LongText(string column)
        {
            return new MigrationColumn();
        }

        public ITableColumn Bool(string column)
        {
            return new MigrationColumn();
        }

        public ITableColumn Timestamp(string column)
        {
            return new MigrationColumn();
        }

        public void Timestamps()
        {
        }

        public void SoftDeletes()
        {

        }
        public void Comment(string comment)
        {

        }

    }
}
