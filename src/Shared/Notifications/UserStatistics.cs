using MediatR;
using NetDream.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Shared.Notifications
{
    /// <summary>
    /// 给用户自己看的全面统计
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="includeTags"></param>
    public class UserStatisticsRequest(int userId, string[]? includeTags = null) : INotification
    {

        public int UserId => userId;

        public IList<StatisticsItem> Result { get; private set; } = [];

        public bool IsInclude(string tag)
        {
            return includeTags is null || includeTags.Contains(tag);
        }

        public void Add(StatisticsItem item)
        {
            Result.Add(item);
        }

        public void Add(string name, int count, string unit)
        {
            Add(new StatisticsItem(name, count, unit));
        }

        public void Add(string name, int count)
        {
            Add(new StatisticsItem(name, count));
        }

        public void TryAdd(string tag, Func<StatisticsItem> cb)
        {
            if (IsInclude(tag))
            {
                Result.Add(cb.Invoke());
            }
        }
    }

    /// <summary>
    /// 公开的统计
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="includeTags"></param>
    public class UserOpenStatisticsRequest(int userId, string[]? includeTags = null) : INotification
    {

        public int UserId => userId;

        public Dictionary<string, object> Result { get; private set; } = [];

        public bool IsInclude(string tag)
        {
            return includeTags is null || includeTags.Contains(tag);
        }

        public void TryAdd(string tag, Func<object> cb)
        {
            if (IsInclude(tag))
            {
                Result.TryAdd(tag, cb.Invoke());
            }
        }
    }
}
