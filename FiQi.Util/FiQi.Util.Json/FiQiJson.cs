using System;
using System.Text.Json;

namespace FiQi.Util.Json
{
    public static class FiQiJson
    {
        public static T DeSerialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }

        public static string Serialize<T>(T t)
        {
            return JsonSerializer.Serialize(t);
        }

        public static bool IsJson(string json)
        {
            throw new NotImplementedException();
        }
    }
}
