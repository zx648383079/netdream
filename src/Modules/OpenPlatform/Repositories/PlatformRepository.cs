using NetDream.Modules.OpenPlatform.Entities;
using NetDream.Shared.Repositories;
using NPoco;

namespace NetDream.Modules.OpenPlatform.Repositories
{
    public class PlatformRepository(IDatabase db) : CRUDRepository<PlatformEntity>(db)
    {
        public const byte STATUS_NONE = 0;
        public const byte STATUS_WAITING = 9;
        public const byte STATUS_SUCCESS = 1;
    }
}
