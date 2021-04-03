using NetDream.Web.Areas.Blog.Entities;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Web.Areas.Blog.Models
{
    public class TagModel: TagEntity
    {
        [Ignore]
        public string FontSize
        {
            get
            {
                return (BlogCount + 12) + "px";
            }
        }
    }
}
