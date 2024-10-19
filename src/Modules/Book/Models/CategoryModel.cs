using NetDream.Modules.Book.Entities;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Book.Models
{
    public class CategoryModel: CategoryEntity
    {
        [Ignore]
        public int BookCount { get; set; }

        [Ignore]
        public string Thumb { get; set; } = string.Empty;
    }
}
