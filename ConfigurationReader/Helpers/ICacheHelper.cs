using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationReader.Helpers
{
    public interface ICacheHelper
    {
        void Set<T>(string key, T value, int timeout = 60);

        T Get<T>(string key);

        bool Remove(string key);

        bool IsInCache(string key);
    }
}
