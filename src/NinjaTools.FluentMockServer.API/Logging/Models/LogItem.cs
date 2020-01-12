using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NinjaTools.FluentMockServer.API.Extensions;

namespace NinjaTools.FluentMockServer.API.Logging.Models
{
    [DebuggerDisplay("{DebuggerDisplay(),nq}")]
    public abstract class LogItem<T> : ILogItem
    {
        /// <inheritdoc />
        [JsonProperty(Order = 0)]
        public string Id { get; }

        [JsonProperty(Order = 1)]
        public abstract LogKind Kind { get; }

        [JsonProperty(Order = 2)]
        public T Content { get; }

        protected LogItem(string id, T content)
        {
            Content = content;
            Id = id;
        }

        protected virtual string FormatContent()
        {
            var json = Serialize(Content);
            return json;
        }

        protected string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                Converters = {new StringEnumConverter()},
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        /// <inheritdoc />
        public virtual string ToFormattedString()
        {
            var header = SafeFormatHeader();
            var content = FormatContent();

            var sb = new StringBuilder();
            sb.AppendLine(header);
            sb.AppendLine(content);
            var message = sb.ToString();
            return message;
        }

        private string SafeFormatHeader()
        {
            try
            {
                var header = FormatHeader();
                return header;
            }
            catch (Exception e)
            {
                return GetType().Name;
            }
        }
        protected virtual string FormatHeader()
        {
            var description = Kind.GetDescription();
            var header = description + $" Id={Id};";
            return header;
        }

        [ExcludeFromCodeCoverage]
        [DebuggerStepThrough]
        public virtual string DebuggerDisplay()
        {
            return $"Id={Id}; Kind={Kind.ToString()};\n{ToFormattedString()}";
        }
    }
}
