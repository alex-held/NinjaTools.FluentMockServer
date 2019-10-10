using HardCoded.MockServer.Fluent.Builder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HardCoded.MockServer.Fluent.FluentExtensions
{
    public static class FluentBodyBuilderExtensions
    {
        public static void WithJsonArray<T>(this IFluentBodyBuilder builder, params T[] items)
        {
          
            builder.WithJson(JsonConvert.SerializeObject(items, Formatting.Indented));
        }
        
        public static void WithJsonContent<T>(this IFluentBodyBuilder builder, T item)
        {
            if (item is string json)
            {
                builder.WithJson(json);
                return;
            }

            builder.WithJson(JsonConvert.SerializeObject(item, Formatting.Indented));
        }
    }
}