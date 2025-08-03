namespace NetDream.Shared.Notifications
{
    public record ReportRequest(byte ItemType, int ItemId, string Title, string Content, int Whistleblower, string Ip, int Timestamp)
    {
    }
}
