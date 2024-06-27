using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Forum.Forms
{
    public class ThreadForm
    {
        public int Id { get; set; }
        public int ForumId { get; set; }
        public int ClassifyId { get; set; }
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public byte IsPrivatePost { get; set; }
    }
}
