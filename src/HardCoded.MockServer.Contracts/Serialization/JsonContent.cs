using System.Net.Http;
using System.Text;

using HardCoded.MockServer.Contracts.Attributes;

using Newtonsoft.Json;

namespace HardCoded.MockServer.Contracts.Serialization
{
    public class JsonContent : StringContent
    {
        /// <inheritdoc />
        public JsonContent([NotNull] object content) : base(JsonConvert.SerializeObject(content, Formatting.Indented, new CustomJsonSerializerSettings()), Encoding.UTF8, CommonContentType.Json)
        {
        }
    }
}
