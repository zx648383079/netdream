using System;

namespace NetDream.Modules.UserAccount.Models
{
    public class UserListItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public int Sex { get; set; }
        public string Avatar { get; set; } = string.Empty;
        public DateOnly Birthday { get; set; }
        public int Money { get; set; }
        public int Credits { get; set; }

        public int ParentId { get; set; }
        public int Status { get; set; }

        public int UpdatedAt { get; set; }

        public int CreatedAt { get; set; }
    }
}
