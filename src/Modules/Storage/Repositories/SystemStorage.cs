using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Storage.Repositories
{
    public class SystemStorage(IEnvironment environment) : ISystemStorage
    {
        public IStorageFolder Open => new StorageFolder(environment.PublicRoot);

        public IStorageFolder Secret => new StorageFolder(environment.OnlineDiskRoot);

        public IStorageFolder Temporary => new StorageFolder(environment.CacheRoot);

        public IStorageFolder Backup => new StorageFolder(environment.BackupRoot);
    }
}
