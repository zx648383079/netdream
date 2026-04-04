using NetDream.Shared.Models;

namespace NetDream.Shared.Interfaces
{
    public interface IApplyRepository
    {
        public void ReceiveCancel(int user, ModuleTargetType type, int target);

        public void ReceiveClear(ModuleTargetType type, int target);

        public void Receive(int user, ModuleTargetType type, int target, ReviewStatus status);

        public void ReceiveCreate(int user, ModuleTargetType type, int target, string remark);


        public IPage<IApplyListItem> ReceiveSearch(ModuleTargetType type, int target, IPaginationForm form);

        public int ReceiveUnread(ModuleTargetType type, int target, int lastAt = 0);

        public bool ReceiveAny(int user, ModuleTargetType type, int target);
    }
}
