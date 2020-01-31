using System.Collections.Generic;
using Newtonsoft.Json;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace NinjaTools.FluentMockServer.API.Models
{
    [JsonObject(MemberSerialization.OptOut, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class RequestMatcher
    {

        [YamlMember(SerializeAs = typeof(Dictionary<string, object>))]
        public RequestBodyMatcher BodyMatcher { get; set; }

        [YamlMember(SerializeAs = typeof(string), ScalarStyle = ScalarStyle.Any)]
        public Path Path { get; set; }

        [YamlMember(SerializeAs = typeof(string), ScalarStyle = ScalarStyle.Any)]
        public Method Method { get; set; }

        [YamlMember(SerializeAs = typeof(Dictionary<string, string[]>))]
        public Headers Headers { get; set; }

        [YamlMember(SerializeAs = typeof(Dictionary<string, string>))]
        public Cookies Cookies { get; set; }

        [YamlMember(SerializeAs = typeof(Dictionary<string, string[]>))]
        public Query Query { get; set; }
    }



}
