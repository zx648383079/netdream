using NetDream.Modules.Contact.Entities;
using NetDream.Shared.Interfaces.Entities;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Contact.Models
{
    public class ReportModel: ReportEntity
    {

        [Ignore]
        public IUser? User { get; set; }
    }
}
