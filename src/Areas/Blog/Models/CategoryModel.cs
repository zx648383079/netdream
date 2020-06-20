using NetDream.Areas.Blog.Entities;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Areas.Blog.Models
{
    public class CategoryModel: CategoryEntity
    {
        [Ignore]
        public int BlogCount { get; set; } = 0;
    }
}
