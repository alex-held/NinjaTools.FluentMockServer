using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace HardCoded.MockServer.Contracts.Serialization
{
    public class HttpMethodConverter : JsonConverter<HttpMethod>
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer,
                                       HttpMethod value,
                                       JsonSerializer serializer)
        {
            writer.WriteValueAsync(value.ToString().ToUpper());
        }

        /// <inheritdoc />
        public override HttpMethod ReadJson(JsonReader reader,
                                            Type objectType,
                                            HttpMethod existingValue,
                                            bool hasExistingValue,
                                            JsonSerializer serializer)
        {
            
            var value = reader.ReadAsString();
    
            if (Is(HttpMethod.Delete)) return HttpMethod.Delete;
            if (Is(HttpMethod.Head)) return HttpMethod.Head;
            if (Is(HttpMethod.Get)) return HttpMethod.Get;
            if (Is(HttpMethod.Patch)) return HttpMethod.Patch;
            if (Is(HttpMethod.Post)) return HttpMethod.Post;
            if (Is(HttpMethod.Put)) return HttpMethod.Put;
            if (Is(HttpMethod.Trace)) return HttpMethod.Trace;
            if (Is(HttpMethod.Options)) return HttpMethod.Options;
            
            throw new JsonSerializationException($"Unable to deserialize json '{value}' to {nameof(HttpMethod)}");

            bool Is(HttpMethod method)
            {
                return string.Equals(value, method.ToString(), StringComparison.CurrentCultureIgnoreCase);
            }
        }
    }
}