using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Core.Interfaces
{
    public interface IClientEnvironment
    {
        public string Ip { get; }
        public string UserAgent { get; }

        public int PlatformId { get; }

        public int UserId { get; }

        public string ClientName { get; }

        public int Now { get; }
    }
}
