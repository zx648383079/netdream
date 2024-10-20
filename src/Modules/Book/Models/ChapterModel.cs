using NetDream.Modules.Book.Entities;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Book.Models
{
    public class ChapterModel: ChapterEntity
    {
        [Ignore]
        public string Content { get; set; } = string.Empty;
    }
}
