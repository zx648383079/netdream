using NetDream.Shared.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Shared.Interfaces.Database
{
    public interface IMigration
    {
        public void Up();

        public void Down();
        public void Seed();

        public IMigration Append<T>(Action<MigrationTable> cb) where T : class;

        public IMigration Append(string tableName, Action<MigrationTable> cb);
    }
}
