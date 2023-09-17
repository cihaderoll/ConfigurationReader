using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConfigurationReader.Helpers
{
    public class TypeHelper
    {
        public static TResult GetValueWithType<TResult>(string val)
        {
            var result = JsonConvert.DeserializeObject<TResult>(val, new JsonSerializerSettings { });

            return result;
        }
    }
}
