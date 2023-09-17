using System;

namespace Configuration.Web.Helpers
{
    public interface ICacheHelper
    {
        void Set<T>(string key, T value, int timeout = 60);

        T Get<T>(string key);

        bool Remove(string key);

        bool IsInCache(string key);
    }
}
