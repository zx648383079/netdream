using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Navigation.Forms
{
    public class SiteForm
    {
        public int Id { get; set; }
        public string Schema { get; set; } = string.Empty;
        [Required]
        public string Domain { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int CatId { get; set; }

        public int TopType { get; set; }

        public string[] Tags { get; set; }

        public string Keywords { get; set; }

        public byte AlsoPage { get; set; }
    }
}
