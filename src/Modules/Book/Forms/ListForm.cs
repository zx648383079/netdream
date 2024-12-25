using NetDream.Modules.Book.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.Book.Forms
{
    public class ListForm
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<ListItemEntity> Items { get; set; }
    }
}
