using NetDream.Modules.Book.Entities;

namespace NetDream.Modules.Book.Models
{
    public class ListItemModel: ListItemEntity
    {
        public BookEntity? Book { get; set; }
        public bool IsAgree { get; set; }
        public bool OnShelf { get; set; }
    }
}
