using NetDream.Shared.Interfaces;
using System;

namespace NetDream.Modules.Shop.Entities
{
    
    public class ActivityTimeEntity : IIdEntity
    {
        
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        
        public TimeSpan StartAt { get; set; }
        
        public TimeSpan EndAt { get; set; }
    }
}
