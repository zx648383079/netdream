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

    public record UserAction(
        int UserId,
        string Action,
        string Remark, 
        ModuleTargetType ItemType,
        int ItemId, string Ip, int Timestamp) : INotification
    {

        public static UserAction Create(
            IClientContext client,
            string action,
            string remark,
            ModuleTargetType itemType,
            int itemId)
        {
            return new(client.UserId, action, remark, itemType, itemId, client.Ip, client.Now);
        }

        public static UserAction Create(
            IClientContext client,
            string action,
            string remark)
        {
            return new(client.UserId, action, remark, ModuleTargetType.User, client.UserId, client.Ip, client.Now);
        }
    }
}
