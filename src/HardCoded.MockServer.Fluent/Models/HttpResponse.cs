using System.Net;
using HardCoded.MockServer.Fluent.Builder;
using HardCoded.MockServer.Fluent.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HardCoded.MockServer.Fluent.Models
{
   
    
    public class HttpResponse
    {
        [JsonProperty("delay", NullValueHandling = NullValueHandling.Ignore)] 
        public HttpResponseBuilder.Delay DelayTime { get; set; }

        [JsonProperty("connectionOptions", NullValueHandling = NullValueHandling.Ignore)] 
        public HttpResponseBuilder.ConnectionOptions Options { get; set; }

        [JsonProperty("statusCode")] 
        public int StatusCode { get; set; }
        
        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public HttpResponseBuilder.ResponseBody Body { get; set; }


        public static HttpResponseBuilder.IBlank Configure() => new HttpResponseBuilder();
        
        public class HttpResponseBuilder : HttpResponseBuilder.IBuildable, HttpResponseBuilder.IBlank
        {

            public interface IBlank
            {
                IBuildable WithStatusCode(HttpStatusCode code);
            }
            
            public class ResponseBody : HttpRequest.HttpRequestBuilder.Body
            {
                [JsonProperty("contentType")]  
                public string ContentType { get; set; }
        
            }
            
            public interface IBuildable
            {
                HttpResponse Build();
                IBuildable WithResponseBody(ResponseBody body);
                IBuildable WithDelay(Delay delay);
            }
            
            public IBuildable WithResponseBody(ResponseBody body)
            {
                Body = body;
                return this;
            }

            /// <inheritdoc />
            public HttpResponse Build()
            {
                return new HttpResponse
                {
                    Body = Body,
                    DelayTime = DelayTime,
                    StatusCode = StatusCode,
                    Options = Options
                };
            }
            public IBuildable WithStatusCode(HttpStatusCode code)
            {
                StatusCode = (int) code;
                return this;
            }
            
            public IBuildable WithDelay(Delay delay)
            {
                DelayTime = delay;
                return this;
            }
            
            private ResponseBody Body { get; set; }
            private Delay DelayTime { get; set; }

            private ConnectionOptions Options { get; set; }
            private int StatusCode { get; set; }

            
            public class ConnectionOptions
            {
                [JsonProperty("closeSocket")] 
                public bool CloseSocket { get; set; }

                [JsonProperty("contentLengthHeaderOverride")]
                public long ContentLengthHeaderOverride { get; set; }

                [JsonProperty("suppressContentLengthHeader")]
                public bool SuppressContentLengthHeader { get; set; }

                [JsonProperty("suppressConnectionHeader")]
                public bool SuppressConnectionHeader { get; set; }

                [JsonProperty("keepAliveOverride")] 
                public bool KeepAliveOverride { get; set; }
            }

            [JsonConverter(typeof(StringEnumConverter))]
            public enum TimeUnit
            {
                SECONDS,
                MILLISECONDS,
                DAYS,
                HOURS,
                MICROSECONDS,
                MINUTES,
                NANOSECONDS
            }

            public class Delay
            {
                public Delay()
                {
                    TimeUnit = TimeUnit.SECONDS;

                }

                private Delay(int delay, TimeUnit unit)
                {
                    TimeUnit = unit;
                    Value = delay;
                }

                public static Delay FromSeconds(int seconds) =>
                    new Delay(seconds, TimeUnit.SECONDS);

                public static Delay FromMinutes(int minutes) =>
                    new Delay(minutes, TimeUnit.MINUTES);


                [JsonProperty("timeUnit")] 
                public TimeUnit TimeUnit { get; set; }

                [JsonProperty("value")] 
                public int Value { get; set; }

                public static Delay None => FromSeconds(0);
            }
        }
    }
    
}