using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.OnlineDisk.Models
{
    public class ServerListItem
    {
        public int Id { get; set; }
        public string Ip { get; set; } = string.Empty;

        public int FileCount { get; set; }
        public byte Status { get; set; }

        public int UpdatedAt { get; set; }

        public int CreatedAt { get; set; }
    }
}
