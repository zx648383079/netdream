using System;
using System.Diagnostics.CodeAnalysis;

namespace NetDream.Shared.Interfaces
{
    public interface ISessionStorage
    {
        public bool TryGet<T>(string key, [NotNullWhen(true)] out T? result);
        public void Set<T>(string key, T value);
        public void Set<T>(string key, T value, TimeSpan absoluteExpirationRelativeToNow);
        public void Set<T>(string key, T value, DateTimeOffset absoluteExpiration);

        public T GetOrSet<T>(string key, Func<T> createFn);
        public void Remove(string key);
    }
}
