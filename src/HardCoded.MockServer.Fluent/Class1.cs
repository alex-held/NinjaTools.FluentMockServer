using System.Net;
using System.Net.Http;
using HardCoded.MockServer.Fluent.Builder;
using HardCoded.MockServer.Fluent.Models;

namespace HardCoded.MockServer.Fluent
{
    public class Class1
    {
        public class Car
        {
            public int PS { get; set; }
            public string Brand { get; set; }
        }
        
        public static void Main()
        {
            MockServerSetup
               .Expectations
               .OnHandling(() => HttpRequest.Configure()
                                            .WithMethod(HttpMethod.Post)
                                            .WithPath("/api/cars")
                                            .KeepConnectionAlive()
                                            .WithJsonArray(new Car {Brand = "Ford", PS = 300})
                                            .Build()
                )
               .Attach()
               .RespondingWith(() => HttpResponse.Configure()
                                                 .WithStatusCode(HttpStatusCode.OK)
                                                 .Build())
               .Apply();
        }
    }
}