using NetDream.Modules.Bot.Entities;

namespace NetDream.Modules.Bot.Models
{
    public class HistoryListItem : MessageHistoryEntity
    {
        public UserLabelItem? FromUser {  get; set; }
        public UserLabelItem? ToUser {  get; set; }
    }
}
