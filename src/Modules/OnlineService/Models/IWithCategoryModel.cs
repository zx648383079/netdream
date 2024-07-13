using Modules.OnlineService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.OnlineService.Models
{
    public interface IWithCategoryModel
    {
        public int CatId { get; }

        public CategoryEntity? Category { set; }
    }
}
