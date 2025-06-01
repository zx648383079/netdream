namespace NetDream.Modules.UserIdentity.Models
{
    public class UserEquityCard
    {
        public int Id { get; set; }

        public int Status { get; set; }
        public string ExpiredAt { get; set; } = string.Empty;
        public int Exp { get; internal set; }
        public string Name { get; internal set; }
        public string Icon { get; internal set; }
    }
}
