using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Legwork.Forms
{
    public class ServiceForm
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CatId { get; set; }
        public string Thumb { get; set; } = string.Empty;
        public string Brief { get; set; } = string.Empty;
        public float Price { get; set; }
        public string Content { get; set; } = string.Empty;
        public ServiceFormItem[] Form { get; set; }
        public int UpdatedAt { get; set; }
        public int CreatedAt { get; set; }
    }
}
