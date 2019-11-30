using System.Net.Http;
using System.Text;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Builders;
using NinjaTools.FluentMockServer.Serialization;

namespace NinjaTools.FluentMockServer.Utils
{
    public class JsonContent : StringContent
    {
        /// <inheritdoc />
        public JsonContent([NotNull] object content) : base(Serializer.Serialize(content), Encoding.UTF8, CommonContentType.Json)
        {
        }
    }
}
