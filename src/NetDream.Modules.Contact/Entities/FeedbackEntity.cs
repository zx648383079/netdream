using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Contact.Entities
{
    [TableName("cif_feedback")]
    public class FeedbackEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
        public int CreatedAt { get; set; }
        public int UpdatedAt { get; set; }
    }
}
