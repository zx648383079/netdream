using MediatR;
using NetDream.Shared.Interfaces;

namespace NetDream.Shared.Notifications
{
    public record ManageAction(
        string Action, 
        string Remark, int ItemType, 
        int ItemId, int UserId, string Ip, int CreateAt) : INotification
    {

        public static ManageAction Create(
            IClientContext client,
            string action,
            string remark, 
            int itemType,
            int itemId)
        {
            return new(action, remark, itemType, itemId, client.UserId, client.Ip, client.Now);
        }
    }
}
