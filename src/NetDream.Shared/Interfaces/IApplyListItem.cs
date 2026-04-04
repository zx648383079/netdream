using NetDream.Shared.Models;

namespace NetDream.Shared.Interfaces
{
    public interface IApplyListItem : IWithUserModel, IIdEntity, ICreatedEntity
    {
        public string Remark { get; }

        public ReviewStatus Status { get; }
    }
}
