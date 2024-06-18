using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Core.Interfaces.Database
{
    public interface ITable
    {
        public ITableColumn Id(string column = "id");

        public ITableColumn Enum(string column, IEnumerable<string> items);

        public ITableColumn String(string column, int length = 255);

        public ITableColumn Uint(string column, int length = 10);

        public ITableColumn Decimal(string column, int length = 16, int d = 10);
        public ITableColumn Float(string column, int length = 16, int d = 10);
        public ITableColumn Double(string column, int length = 16, int d = 10);

        public ITableColumn Char(string column, int length = 20);

        public ITableColumn Bool(string column);

        public ITableColumn Int(string column);

        public ITableColumn Date(string column);

        public ITableColumn Time(string column);
        public ITableColumn Text(string column);
        public ITableColumn MediumText(string column);
        public ITableColumn LongText(string column);

        public ITableColumn Timestamp(string column);

        public void Timestamps();

        public void SoftDeletes();

        public void Comment(string comment);
    }
}
