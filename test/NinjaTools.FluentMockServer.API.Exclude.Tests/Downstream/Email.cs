using System;
using System.Text;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NinjaTools.FluentMockServer.API.Tests.Downstream
{
    public class Email
    {
        public string To { get; set; }
        public string From { get; set; } 
        public string Subject { get; set; }
        public string Content { get; set; }


        /// <inheritdoc />
        [NotNull]
        public override string ToString()
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("Start of message");
                // sb.AppendLine();
                sb.AppendLine($"From:\t{From}");
                sb.AppendLine($"To:\t{To}");
                //  sb.AppendLine();
                sb.AppendLine($"Subject: {Subject}");
                // sb.AppendLine();
                sb.AppendLine("Content:");
                //  sb.AppendLine();
                sb.AppendLine(JObject.FromObject(Content).ToString(Formatting.Indented));
                // sb.AppendLine();
                sb.AppendLine("End of message");

                return sb.ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
