using NetDream.Modules.Book.Entities;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Book.Models
{
    public class ListItemModel: ListItemEntity
    {
        [Ignore]
        public BookEntity? Book { get; set; }
        [Ignore]
        public bool IsAgree { get; set; }
        [Ignore]
        public bool OnShelf { get; set; }
    }
}
