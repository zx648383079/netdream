using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Shared.Repositories.Models
{
    public class FileItem(string name)
    {
        public string Name { get; set; } = name;

        public long Size { get; set; }

        public int CreatedAt { get; set; }
    }
}
