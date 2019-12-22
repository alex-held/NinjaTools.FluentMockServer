using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using Bogus;
using Bogus.DataSets;
using Bogus.Extensions;

using FluentAssertions;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Client.Models.ValueTypes;
using NinjaTools.FluentMockServer.Domain.Models.HttpEntities;
using Xunit;
using Xunit.Abstractions;


namespace NinjaTools.FluentMockServer.Tests.Contracts
{

    public static class BogusWebExtensions
    {
        public static readonly List<HttpMethod> HttpMethods = new List<HttpMethod>
        {
                    HttpMethod.Delete
                  , HttpMethod.Head
                  , HttpMethod.Trace
                  , HttpMethod.Options
                  , HttpMethod.Patch
                  , HttpMethod.Put
                  , HttpMethod.Post
        };


        /// <summary>Generate a random <see cref="HttpMethod" />.</summary>
        /// <param name="separator">The string value to separate the obfuscated credit card.</param>
        public static HttpMethod RandomHttpMethod
                    (this Internet internet) => internet.Random.CollectionItem(HttpMethods);
    }

    public class RandomData
    {
        public enum Gender
        {
            Male, Female
        }

        public class RantCollection
        {
            public string Owner { get; set; }
            public Gender Gender { get; set; }
            public List<RandRecord> Rants { get; set; } = new List<RandRecord>();
            public Guid? Id { get; set; }
        }

        public class RandRecord
        {
            public string Person { get; set; }
            public string Rant { get; set; }
            public DateTime? Date { get; set; }
            public int RantId { get; set; }
        }


        public List<RantCollection> RandomRantCollectionGenerator(int count, int? size = null)
        {
            var quoteFaker = new Faker<RandRecord>()
                        .RuleFor(x => x.RantId, f => f.Random.Int(1, 500000))
                        .RuleFor(x => x.Person, f => f.Internet.UserNameUnicode())
                        .RuleFor(x => x.Rant,   f => f.Rant.Review(new Faker().Commerce.ProductName()))
                        .RuleFor(x => x.Date,   f => f.Date.Between(DateTime.UnixEpoch, DateTime.UtcNow));

            var faker = new Faker<RantCollection>()
                        .RuleFor(x => x.Id,     f => f.Random.Guid().OrNull(new Faker()))
                        .RuleFor(x => x.Owner,  f => f.Person.FullName)
                        .RuleFor(x => x.Gender, f => f.PickRandom<Gender>())
                        .RuleFor(x => x.Rants,  f => quoteFaker.Generate(new Faker().Random.Int(10, size ?? 50)));

            faker.AssertConfigurationIsValid();

            return faker.Generate(count);
        }


        /// <summary>Gets some test data.</summary>
        /// <returns></returns>
        public RantCollection GetRandomRantCollection()
        {
            var quoteFaker = new Faker<RandRecord>()
                        .RuleFor(x => x.RantId, f => f.Random.Int(1, 500000))
                        .RuleFor(x => x.Person, f => f.Internet.UserNameUnicode())
                        .RuleFor(x => x.Rant,   f => f.Rant.Review(new Faker().Commerce.ProductName()))
                        .RuleFor(x => x.Date,   f => f.Date.Between(DateTime.UnixEpoch, DateTime.UtcNow));

            var faker = new Faker<RantCollection>()
                        .RuleFor(x => x.Id,     f => f.Random.Guid().OrNull(new Faker()))
                        .RuleFor(x => x.Owner,  f => f.Person.FullName)
                        .RuleFor(x => x.Gender, f => f.PickRandom<Gender>())
                        .RuleFor(x => x.Rants,  f => quoteFaker.Generate(new Faker().Random.Int(10, 50)));

            faker.AssertConfigurationIsValid();

            return faker.Generate();
        }


        /// <summary>Gets a random request body.</summary>
        /// <returns></returns>
        public Body GetRandomRequestBody()
        {
            var faker = new Faker<Body>()
                        .CustomInstantiator(faker => new Body())
                        .RuleFor(x => x.Path, () =>
                                        JObject.FromObject(GetRandomRantCollection())
                                                    .ToString(Formatting.Indented))
                        .RuleFor(x => x.Path, Body.BodyType.JSON.ToString());

            faker.AssertConfigurationIsValid();

            return faker.Generate();
        }


        /// <summary>Gets a random <see cref="HttpRequest" />.</summary>
        /// <returns></returns>
        public HttpRequest GetRandomHttpRequest()
        {
            // 1) Data to fill the RequestBody
            // -> GetRandomRantCollection

            // 2) The RequestBody

            var faker = new Faker<HttpRequest>()
                        .RuleFor(req => req.Method, f => f.Internet.RandomHttpMethod().ToString())
                        .RuleFor(x => x.Path,       f => f.Internet.UrlWithPath())
                        .RuleFor(x => x.Body,       GetRandomRequestBody);

            return faker.Generate();
        }
    }

    public class RandomHttpRequestTestData : TheoryData<HttpRequest>
    {
        public RandomHttpRequestTestData()
        {
            var data = new RandomData();

            Enumerable.Range(0, 10)
                        .Select(_ => data.GetRandomHttpRequest())
                        .ToList().ForEach(
                            req => {
                                Add(req);
                            });
        }
    }

    public class SerializerTests
    {
        private readonly ITestOutputHelper _output;


        public SerializerTests(ITestOutputHelper output) => _output = output;


        [Theory]
        [ClassData(typeof(RandomHttpRequestTestData))]
        public void Should_Serialize_HttpRequests_As_Expected(HttpRequest request)
        {
            // Act & Assert
            var json = request.ToString();
            _output.WriteLine(json);
        }
    }

    public class HttpMethodConverterTests
    {
        private readonly ITestOutputHelper _output;


        public HttpMethodConverterTests(ITestOutputHelper output) => _output = output;


        public class HttpMethodTestData : TheoryData<HttpMethod, string>
        {
            public HttpMethodTestData()
            {
                Add(HttpMethod.Delete,  "DELETE");
                Add(HttpMethod.Put,     "PUT");
                Add(HttpMethod.Post,    "POST");
                Add(HttpMethod.Get,     "GET");
                Add(HttpMethod.Head,    "HEAD");
                Add(HttpMethod.Options, "OPTIONS");
                Add(HttpMethod.Patch,   "PATCH");
                Add(HttpMethod.Trace,   "TRACE");
            }
        }

        public class PersonB 
        {
            
            public Guid ID { get; }


            public PersonB() => Guid.NewGuid();


            public string Name { get; set; } = "Ralf";
            public string City { get; set; }
            public bool? Homeoffice { get; set; }
            public List<string> Hobbies { get; set; }
        }

        public class PersonA
        {
            public Guid ID { get; }


            public PersonA() => Guid.NewGuid();


            public string Name { get; set; } = "Alex";
            public string City { get; set; }
            public bool? Homeoffice { get; set; }
            public List<string> Hobbies { get; set; }
        }


        [Fact]
        public void Should_Serialize_Derived_Classes()
        {
            var p1 = new PersonA();
            var p2 = new PersonB();

            var jn1 = JObject.FromObject(p1);
            var json1 = jn1.ToString(Formatting.Indented);
            _output.WriteLine($"PersonA: \n---\n{json1}\n---\n");

            var jo2 = JObject.FromObject(p2);
            var json2 = jo2.ToString(Formatting.Indented);
            _output.WriteLine($"PersonB: \n---\n{json2}\n---\n");

            JToken.DeepEquals(jn1, jo2).Should().BeFalse();
            json2.Should().NotBe(json1);
        }
    }
}