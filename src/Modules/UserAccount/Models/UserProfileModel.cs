using NetDream.Modules.UserAccount.Entities;
using NetDream.Shared.Converters;
using NetDream.Shared.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NetDream.Modules.UserAccount.Models
{
    /// <summary>
    /// 自己登录时的数据
    /// </summary>
    public class UserProfileModel : IUserProfile, IUserToken
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

        public int UpdatedAt { get; set; }

        public int CreatedAt { get; set; }

        public string Background { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        [JsonIgnore]
        public int BulletinCount => MetaItems?.TryGetValue("bulletin_count", out var i) == true ? (int)i : 0;

        [JsonMeta]
        public Dictionary<string, object>? MetaItems { get; set; }

        public UserProfileModel()
        {
            
        }

        public UserProfileModel(UserEntity entity)
        {
            Id = entity.Id;
            Email = entity.Email;
            Name = entity.Name;
            Mobile = entity.Mobile;
            Sex = entity.Sex;
            Avatar = entity.Avatar;
            Birthday = entity.Birthday;
            Money = entity.Money;
            Credits = entity.Credits;
            ParentId = entity.ParentId;
            UpdatedAt = entity.UpdatedAt;
            CreatedAt = entity.CreatedAt;
        }
    }
}
