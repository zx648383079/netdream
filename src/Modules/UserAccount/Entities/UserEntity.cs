using NetDream.Shared.Interfaces;
using System;

namespace NetDream.Modules.UserAccount.Entities
{
    
    public class UserEntity: IIdEntity, ITimestampEntity, IUserProfile
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public byte Sex { get; set; }
        public string Avatar { get; set; } = string.Empty;
        public DateOnly Birthday { get; set; }
        
        public int ParentId { get; set; }
        public string Token { get; set; } = string.Empty;
        public byte Status { get; set; }

        public int ActivatedAt { get; set; }

        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
