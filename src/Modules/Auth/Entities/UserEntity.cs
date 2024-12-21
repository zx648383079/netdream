using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Auth.Entities
{
    
    public class UserEntity: IUser
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Sex { get; set; }
        public string Avatar { get; set; } = string.Empty;
        public string Birthday { get; set; } = string.Empty;
        public int Money { get; set; }
        public int Credits { get; set; }
        
        public int ParentId { get; set; }
        public string Token { get; set; } = string.Empty;
        public int Status { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }

        [Ignore]
        public bool IsOnline { get; } = false;
    }
}
