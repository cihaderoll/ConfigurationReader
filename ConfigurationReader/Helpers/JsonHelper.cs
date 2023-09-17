using ConfigurationReader.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationReader.Helpers
{
    public static class JsonHelper
    {
        public static List<ConfigurationDto> GetGonfigListFromJsonFile()
        {
            var serializer = new JsonSerializer();
            List<ConfigurationDto> configs = new();
            using (var streamReader = new StreamReader(Environment.CurrentDirectory + "\\..\\ConfigurationReader\\appconfig.json"))
            using (var textReader = new JsonTextReader(streamReader))
            {
                configs = serializer.Deserialize<List<ConfigurationDto>>(textReader);
            }

            return configs;
        }

        public static void SetAppConfigJsonFileData(List<ConfigurationDto> dataList)
        {
            var strVal = JsonConvert.SerializeObject(dataList, new JsonSerializerSettings { });
            File.WriteAllText(Environment.CurrentDirectory + "\\..\\ConfigurationReader\\appconfig.json", strVal);
        }
    }
}
