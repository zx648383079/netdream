using MediatR;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Shared.Notifications
{
    public record TokenCreated(string Token, IUserProfile User, int ExpireToNow) : INotification
    {
    }

    public record TokenCancel(string Token) : INotification
    {

    }

    public record UserSignIn(string Mode, string Account, bool Status, int UserId, string Ip, int Timestamp) : INotification
    {
        public static UserSignIn Create(IClientContext client, string account, 
            int userId, bool status)
        {
            return new(client.ClientName, account, status, userId, client.Ip, client.Now);
        }

        public static UserSignIn Fail(
            IClientContext client,
            string mode, string account)
        {
            return new(mode, account, false, 0, client.Ip, client.Now);
        }

        public static UserSignIn Fail(
            IClientContext client, string account)
        {
            return Fail(client, client.ClientName, account);
        }

        public static UserSignIn Ok(
            IClientContext client,
            string mode,
            IUserProfile user)
        {
            return new(mode, user.Email, true, user.Id, client.Ip, client.Now);
        }

        public static UserSignIn Ok(
            IClientContext client,
            IUserProfile user)
        {
            return Ok(client, client.ClientName, user);
        }
    }
}
