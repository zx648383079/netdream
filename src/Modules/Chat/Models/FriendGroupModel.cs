using NetDream.Modules.Chat.Entities;
using NetDream.Shared.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Chat.Models
{
    public class FriendGroupModel
    {

        public FriendGroupModel()
        {
            
        }

        public FriendGroupModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public FriendGroupModel(FriendClassifyEntity model)
        {
            Id = model.Id;
            Name = model.Name;
        }

        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Online => Users.Where(i => i.User is IUserSource u && u.IsOnline).Count();

        public int Count => Users.Count;

        public List<FriendModel> Users { get; set; } = [];
    }
}
