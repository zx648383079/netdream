using NetDream.Shared.Models;
using NetDream.Shared.Repositories;

namespace NetDream.Shared.Interfaces
{
    public interface IApplyRepository
    {
        public void ReceiveCancel(int user, ModuleTargetType itemType, int itemId);

        public void ReceiveClear(ModuleTargetType itemType, int itemId);

        public void Receive(int user, ModuleTargetType itemType, int itemId, ReviewStatus status);

        public void ReceiveCreate(int user, ModuleTargetType itemType, int itemId, string remark);


        public IPage<IApplyListItem> ReceiveSearch(ModuleTargetType itemType, int itemId, PaginationForm form);

        public int ReceiveUnread(ModuleTargetType itemType, int itemId, int lastAt = 0);

        public bool ReceiveAny(int user, ModuleTargetType itemType, int itemId);
    }
}
