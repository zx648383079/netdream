using Modules.OnlineService.Entities;
using NPoco;
using System.Collections.Generic;

namespace NetDream.Modules.OnlineService.Models
{
    public class CategoryModel: CategoryEntity
    {
        [Ignore]
        public IList<CategoryWordEntity>? Words { get; set; }
    }
}
