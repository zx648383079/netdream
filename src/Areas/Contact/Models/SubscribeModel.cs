using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Areas.Contact.Models
{
    [TableName("cif_subscribe")]
    public class SubscribeModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public int CreatedAt { get; set; }
        public int UpdatedAt { get; set; }
    }
}
