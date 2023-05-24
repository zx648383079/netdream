using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Cli.Models
{
    public class Configuration
    {
        public ConnectionString ConnectionStrings { get; set; } = new();
    }

    public class ConnectionString
    {
        public string Default { get; set; } = string.Empty;
    }
}
