using MediatR;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Shared.Notifications
{
    public class UserProfileCardRequest(int userId, string[]? includeTags = null) : INotification
    {
        public int UserId => userId;

        public UserProfileCard Result { get; private set; } = new(userId);

        public bool IsInclude(string tag)
        {
            return includeTags is null || includeTags.Contains(tag);
        }

        /// <summary>
        /// 添加统计信息
        /// </summary>
        /// <param name="items">包含 icon 为徽标</param>
        public void Add(params StatisticsItem[] items)
        {
            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(item.Icon))
                {
                    Result.MedalItems.Add(item);
                    continue;
                }
                Result.CountItems.Add(item);
            }
        }

        public void Add(params UserCardItem[] items)
        {
            foreach (var item in items)
            {
                Result.CardItems.Add(item);
            }
        }

        public void Add(IUser user)
        {
            if (user.Id != userId)
            {
                return;
            }
            Result.Avatar = user.Avatar;
            Result.Name = user.Name;
            if (user is IUserSource s)
            {
                Result.IsOnline = s.IsOnline;
            } else if (user is IUserProfile p)
            {
                Result.Sex = p.Sex;
                Result.CreatedAt = TimeHelper.TimestampTo(p.CreatedAt);
            }
        }
    }

    public class UserProfileCard(int id) : IUserSource
    {
        public int Id => id;

        public string Name { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;

        public int Sex { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string LastIp { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public bool IsOnline { get; set; }

        public IList<UserCardItem> CardItems { get; private set; } = [];
        public IList<StatisticsItem> CountItems { get; set; } = [];
        public IList<StatisticsItem> MedalItems { get; set; } = [];
    }


    public class UserCardItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public int Exp { get; set; }
        public byte Status { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
