using NetDream.Shared.Interfaces;
using NetDream.Shared.Repositories;

namespace NetDream.Shared.Notifications
{
    public record ReportRequest(
        ModuleTargetType ItemType, int ItemId, 
        string Title, string Content, 
        int Whistleblower, string Ip, int Timestamp)
    {


        public static ReportRequest Create(IClientContext client,
            ModuleTargetType itemType, int itemId,
            string title, string content)
        {
            return new ReportRequest(
                itemType, itemId, 
                title, content, 
                client.UserId, client.Ip,
                client.Now);
        }
    }
}
