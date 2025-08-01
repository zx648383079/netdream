using NetDream.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.OnlineDisk.Forms
{
    public class CatalogQueryForm : QueryForm
    {
        public int Id { get; set; }

        public string Path { get; set; }
    }
}
