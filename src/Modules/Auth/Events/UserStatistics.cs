using MediatR;
using NetDream.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Auth.Events
{
    /// <summary>
    /// 给用户自己看的全面统计
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="includeTags"></param>
    public class UserStatistics(int userId, string[]? includeTags = null) : IRequest<IEnumerable<StatisticsItem>>
    {

        public int UserId => userId;

        public bool IsInclude(string tag)
        {
            return includeTags is null || includeTags.Contains(tag);
        }
    }

    /// <summary>
    /// 公开的统计
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="includeTags"></param>
    public class UserOpenStatistics(int userId, string[]? includeTags = null) : INotification
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
