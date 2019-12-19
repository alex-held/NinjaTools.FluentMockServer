using System;
using System.Linq;
using Bogus;
using Bogus.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Tests.TestHelpers.Data.Random;
using NinjaTools.FluentMockServer.Tests.TestHelpers.Data.Types;
using Xunit;

namespace NinjaTools.FluentMockServer.Tests.TestHelpers.Data.TheoryData
{
    public class RandomHttpRequestData : TheoryData<HttpRequest>
    {
        public RandomHttpRequestData()
        {
            Enumerable.Range(0, 10)
                .Select(_ => RandomDataGenerator.GetRandomHttpRequest())
                .ToList().ForEach(
                    req => { Add(req); });
        }
    }
    
    public static class RandomDataGenerator
    {
        // private static List<RantCollection> RandomRantCollectionGenerator(int count, int? size = null)
        // {
        //     var quoteFaker = new Faker<RandRecord>()
        //         .RuleFor(x => x.RantId, f => f.Random.Int(1, 500000))
        //         .RuleFor(x => x.Person, f => f.Internet.UserNameUnicode())
        //         .RuleFor(x => x.Rant, f => f.Rant.Review(new Faker().Commerce.ProductName()))
        //         .RuleFor(x => x.Date, f => f.Date.Between(DateTime.UnixEpoch, DateTime.UtcNow));
        //
        //     var faker = new Faker<RantCollection>()
        //         .RuleFor(x => x.Id, f => f.Random.Guid().OrNull(new Faker()))
        //         .RuleFor(x => x.Owner, f => f.Person.FullName)
        //         .RuleFor(x => x.Gender, f => f.PickRandom<Gender>())
        //         .RuleFor(x => x.Rants, f => quoteFaker.Generate(new Faker().Random.Int(10, size ?? 50)));
        //
        //     faker.AssertConfigurationIsValid();
        //
        //     return faker.Generate(count);
        // }


        /// <summary>Gets some test data.</summary>
        /// <returns></returns>
        private static RantCollection GetRandomRantCollection()
        {
            var quoteFaker = new Faker<RandRecord>()
                .RuleFor(x => x.RantId, f => f.Random.Int(1, 500000))
                .RuleFor(x => x.Person, f => f.Internet.UserNameUnicode())
                .RuleFor(x => x.Rant, f => f.Rant.Review(new Faker().Commerce.ProductName()))
                .RuleFor(x => x.Date, f => f.Date.Between(DateTime.UnixEpoch, DateTime.UtcNow));

            var faker = new Faker<RantCollection>()
                .RuleFor(x => x.Id, f => f.Random.Guid().OrNull(new Faker()))
                .RuleFor(x => x.Owner, f => f.Person.FullName)
                .RuleFor(x => x.Gender, f => f.PickRandom<Gender>())
                .RuleFor(x => x.Rants, f => quoteFaker.Generate(new Faker().Random.Int(10, 50)));

            faker.AssertConfigurationIsValid();

            return faker.Generate();
        }
        
        
        /// <summary>Gets a random request body.</summary>
        /// <returns></returns>
        public static Body GetRandomRequestBody()
        {
            var faker = new Faker<Body>()
                .CustomInstantiator(fake => new Body())
                .RuleFor(x => x.Path, () =>
                    JObject.FromObject(GetRandomRantCollection())
                        .ToString(Formatting.Indented))
                .RuleFor(x => x.Path, Body.BodyType.JSON.ToString());

            faker.AssertConfigurationIsValid();

            return faker.Generate();
        }


        /// <summary>Gets a random <see cref="HttpRequest" />.</summary>
        /// <returns></returns>
        public static HttpRequest GetRandomHttpRequest()
        {
            // 1) Data to fill the RequestBody
            // -> GetRandomRantCollection

            // 2) The RequestBody
            var faker = new Faker<HttpRequest>()
                .CustomInstantiator(f => HttpRequest.Create(
                    method: f.Internet.RandomHttpMethod().ToString(),
                    path: f.Internet.UrlWithPath(),
                    body: GetRandomRequestBody()));

            return faker.Generate();
        }
    }


 
}
