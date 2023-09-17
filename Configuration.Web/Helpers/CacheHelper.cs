using ServiceStack.Redis;
using System;

namespace Configuration.Web.Helpers
{
    public class CacheHelper : ICacheHelper
    {
        private readonly RedisEndpoint _endPoint;

        public CacheHelper(string host, int port)
        {
            _endPoint = new RedisEndpoint(host, port);
        }

        public T Get<T>(string key)
        {
            T result = default(T);

            using (RedisClient client = new RedisClient(_endPoint))
            {
                result = client.As<T>().GetValue(key);
            }

            return result;
        }

        public bool IsInCache(string key)
        {
            bool isInCache = false;

            using (RedisClient client = new RedisClient(_endPoint))
            {
                isInCache = client.ContainsKey(key);
            }

            return isInCache;
        }

        public bool Remove(string key)
        {
            bool removed = false;

            using (RedisClient client = new RedisClient(_endPoint))
            {
                removed = client.Remove(key);
            }

            return removed;
        }

        public void Set<T>(string key, T value, int timeout = 60)
        {
            using (RedisClient client = new RedisClient(_endPoint))
            {
                client.As<T>().SetValue(key, value, TimeSpan.FromMinutes(timeout));
            }
        }
    }
}
