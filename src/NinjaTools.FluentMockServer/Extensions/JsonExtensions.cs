using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NinjaTools.FluentMockServer.Extensions
{
    public static class JsonExtensions
    {
        public static JObject AsJObject(this object obj) => JObject.FromObject(obj);
        public static string AsJson(this object obj, bool indent = true) => obj.AsJObject().ToString(indent ? Formatting.Indented : Formatting.None);
    }
}
