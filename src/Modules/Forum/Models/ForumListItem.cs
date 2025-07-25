using NetDream.Modules.Forum.Entities;

namespace NetDream.Modules.Forum.Models
{
    public class ForumListItem : ForumEntity
    {

        public ForumListItem[] Children { get; set; }

        public ThreadEntity LastThread {  get; set; }
        public int TodayCount {  get; set; }
    }
}
