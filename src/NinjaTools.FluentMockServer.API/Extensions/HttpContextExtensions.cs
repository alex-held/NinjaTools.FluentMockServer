using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace NinjaTools.FluentMockServer.API.Extensions
{
    public static class HttpContextExtensions
    {
        public static async Task<T> ReadAsync<T>([NotNull] this HttpRequest httpRequest)
        {
            var reader = new StreamReader(httpRequest.Body);
            var json = await reader.ReadToEndAsync();
            return JObject.Parse(json).ToObject<T>();
        }
    }
}
