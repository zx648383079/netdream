using NetDream.Modules.Book.Entities;
using NetDream.Shared.Interfaces.Entities;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Book.Models
{
    public class ListModel: ListEntity, IWithUserModel
    {

        [Ignore]
        public IUser? User { get; set; }

        [Ignore]
        public bool IsCollected { get; set; }
        [Ignore]
        public List<ListItemModel>? Items { get; set; }
    }
}
