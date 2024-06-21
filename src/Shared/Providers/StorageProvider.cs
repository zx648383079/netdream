using NetDream.Shared.Interfaces.Database;
using NPoco;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Shared.Providers
{
    public class StorageProvider(IDatabase db, string root, int tag) : IMigrationProvider
    {
        public const string FILE_TABLE = "base_file";
        public const string FILE_LOG_TABLE = "base_file_log";
        public const string FILE_QUOTE_TABLE = "base_file_quote";

        public static StorageProvider Store(IDatabase db, string root, int tag = 3)
        {
            return new StorageProvider(db, root, tag);
        }

        public static StorageProvider PublicStore(IDatabase db)
        {
            return Store(db, "view.asset_directory", 1);
        }

        public static StorageProvider PrivateStore(IDatabase db)
        {
            return Store(db, "data/storage", 2);
        }
        public void Migration(IMigration migration)
        {
            throw new NotImplementedException();
        }
    }
}
