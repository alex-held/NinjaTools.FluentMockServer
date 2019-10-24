using System.Net.Http;
using System.Text;

using JetBrains.Annotations;

using Newtonsoft.Json;

using NinjaTools.FluentMockServer.Builders;


namespace NinjaTools.FluentMockServer.Utils
{
    public class JsonContent : StringContent
    {
        /// <inheritdoc />
        public JsonContent([NotNull] object content) : base(JsonConvert.SerializeObject(content, Formatting.Indented, new CustomJsonSerializerSettings()), Encoding.UTF8, CommonContentType.Json)
        {
        }
    }
}
