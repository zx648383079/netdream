using MediatR;
using NetDream.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Events
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
