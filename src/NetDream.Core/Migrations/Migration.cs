using NetDream.Core.Helpers;
using NetDream.Core.Interfaces.Database;
using NPoco;
using System;
namespace NetDream.Core.Migrations
{
    public abstract class Migration(IDatabase db) : IMigration
    {
        public abstract void Up();
        public virtual void Down()
        {

        }
        public virtual void Seed()
        {

        }

        public IMigration Append<T>(Action<MigrationTable> cb) where T : class
        {
            return Append(ModelHelper.TableName<T>(), cb);
        }

        public IMigration Append(string tableName, Action<MigrationTable> cb)
        {
            return this;
        }

        protected void AutoUp()
        {

        }
    }
}
