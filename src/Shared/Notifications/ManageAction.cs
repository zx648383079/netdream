using MediatR;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Repositories;

namespace NetDream.Shared.Notifications
{
    public record ManageAction(
        string Action, 
        string Remark, ModuleTargetType ItemType, 
        int ItemId, int UserId, string Ip, int Timestamp) : INotification
    {

        public static ManageAction Create(
            IClientContext client,
            string action,
            string remark,
            ModuleTargetType itemType,
            int itemId)
        {
            return new(action, remark, itemType, itemId, client.UserId, client.Ip, client.Now);
        }
    }
}
