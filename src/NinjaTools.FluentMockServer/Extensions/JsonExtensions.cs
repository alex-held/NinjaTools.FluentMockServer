using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Serialization;

namespace NinjaTools.FluentMockServer.Extensions
{
    public static class JsonExtensions
    {
        public static JObject AsJObject<T>(this T obj)
        {
            return Serializer.SerializeJObject(obj);
        }

        public static string AsJson(this object obj)
        {
            return Serializer.Serialize(obj);
        }
    }
}
