
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Entities
{
    
    public class PermissionEntity
    {
        
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        
        public string DisplayName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        
        public int UpdatedAt { get; set; }

        
        public int CreatedAt { get; set; }

    }
}
