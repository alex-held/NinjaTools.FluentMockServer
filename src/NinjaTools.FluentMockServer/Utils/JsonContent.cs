using System.Net.Http;
using System.Text;

using JetBrains.Annotations;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.Abstractions;
using NinjaTools.FluentMockServer.Builders;


namespace NinjaTools.FluentMockServer.Utils
{
    public class JsonContent : StringContent
    {
        /// <inheritdoc />
        public JsonContent([NotNull] IBuildable content) : base(content.SerializeJObject().ToString(Formatting.Indented), Encoding.UTF8, CommonContentType.Json)
        {
        }
    }
}
