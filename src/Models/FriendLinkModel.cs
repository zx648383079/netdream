using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Models
{
    [TableName("base_friend_link")]
    public class FriendLinkModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Logo { get; set; }
        public string Brief { get; set; }
        public int Status { get; set; }
        public int CreatedAt { get; set; }
        public int UpdatedAt { get; set; }

        public static List<FriendLinkModel> All()
        {
            var data = new List<FriendLinkModel>();
            data.Add(new FriendLinkModel()
            {
                Name = "小呆导航",
                Url = "http://webjike.com",
            });
            return data;
        }
    }
}
