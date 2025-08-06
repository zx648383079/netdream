namespace NetDream.Modules.Bot.Models
{
    public class UserListItem
    {
        public int Id { get; set; }
        public string Nickname { get; set; } = string.Empty;
        public byte Sex { get; set; }
        public string Avatar { get; set; } = string.Empty;

        public int SubscribeAt { get; set; }

        public string Remark { get; set; } = string.Empty;

        public int GroupId { get; set; }

        public int UpdatedAt { get; set; }

        public int BotId { get; set; }

        public string NoteName { get; set; } = string.Empty;
        public byte Status { get; set; }

        public byte IsBlack { get; set; }

        public int CreatedAt { get; set; }
    }

    public class UserLabelItem
    {
        public int Id { get; set; }
        public string Nickname { get; set; } = string.Empty;
        public string NoteName { get; set; } = string.Empty;

        public string Openid { get; set; } = string.Empty;

        public string Avatar { get; set; }
    }
}
