using System.IO;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Xunit;

namespace NinjaTools.FluentMockServer.API.Tests.Downstream
{
    public class HttpContentEmailData : TheoryData<Stream, HttpContent>
    {
        public const int Count = 5;
        
        /// <inheritdoc />
        public HttpContentEmailData ()
        {
            foreach (var email in RandomEmailFactory.Generate(Count))
            {
                AddDataset(email.ToString());
            }

            // var dataCount = 0;
            // do
            // {
            //     var email = RandomEmailFactory.GenerateOneRandomEmail();
            //     var emailContent = JsonConvert.SerializeObject(email, Formatting.Indented);
            //     AddDataset(emailContent);
            //
            // } while (++dataCount != Count);
        }
        
        private void AddDataset([JetBrains.Annotations.NotNull] string content, [JetBrains.Annotations.CanBeNull] string? other = null)
        {
            Add(new MemoryStream(Encoding.UTF8.GetBytes(content)), new ByteArrayContent(Encoding.UTF8.GetBytes(other ?? content)));
        }
    }
}
