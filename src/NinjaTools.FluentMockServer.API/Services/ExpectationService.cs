using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AutoBogus;
using Bogus;
using Bogus.Extensions;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.API.Data;
using NinjaTools.FluentMockServer.FluentAPI;
using NinjaTools.FluentMockServer.FluentAPI.Builders.ValueObjects;
using NinjaTools.FluentMockServer.Models;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.API.Services
{

    public static class ExpectationFactory
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
        
        
        private static readonly List<Action<IFluentBodyBuilder>> BodyOptions = new List<Action<IFluentBodyBuilder>>
        {
            b => b.WithXmlContent("xml"),
            b => b.ContainingJson("{ \"id\": 1, \"name\": \"A green door\", \"price\": 12.50, \"tags\": [\"home\", \"green\"] }"),
            b => b.WithExactJson("{ \"id\": 1, \"name\": \"A green door\", \"price\": 12.50, \"tags\": [\"home\", \"green\"] }"),
            b => b.WithoutExactContent("{ \"id\": 1, \"name\": \"A green door\", \"price\": 12.50, \"tags\": [\"home\", \"green\"] }"),
            b => b.MatchingJsonSchema("{ \"id\": 1, \"name\": \"A green door\", \"price\": 12.50, \"tags\": [\"home\", \"green\"] }"),
            b => b.WithoutExactJson("{ \"id\": 1, \"name\": \"A green door\", \"price\": 12.50, \"tags\": [\"home\", \"green\"] }")
        };

        
        private static readonly Faker<Expectation> ExpectationFaker =  new AutoFaker<Expectation>()
            .Ignore(exp => exp.Id)
            .RuleFor(exp => exp.HttpResponse, FakeResponse)
            .RuleFor(exp => exp.HttpRequest, GetRandomHttpRequest)
            .Ignore(exp => exp.HttpError);



        private static readonly Faker<HttpRequest> HttpRequestFaker = new AutoFaker<HttpRequest>()
            .RuleFor(req => req.Method, f => f.PickRandom(HttpMethods).ToString().ToUpper())
            .RuleFor(x => x.Path, f => f.Internet.UrlWithPath())
            .RuleFor(req => req.Body, fake => FakeBody())
            .RuleFor(x => x.Secure, fake => fake.Random.Bool().OrNull(fake, 0.8F))
            .RuleFor(x => x.KeepAlive, fake => fake.Random.Bool().OrNull(fake, 0.8F));


        
        
        static ExpectationFactory()
        {
            // Configure globally
            AutoFaker.Configure(builder =>
            {
                builder
                    .WithLocale("en_US") // Configures the locale to use// Configures the number of items in a collection
                    .WithRecursiveDepth(5) // Configures how deep nested types should recurse
                    .WithBinder<AutoBinder>() // Configures the binder to use
                    .WithSkip<Expectation>(exp => exp.HttpForward)
                    .WithSkip<Expectation>(exp => exp.HttpForwardTemplate)
                    .WithSkip<Expectation>(exp => exp.HttpResponseTemplate)
                    .WithSkip<Expectation>(exp => exp.Id)
                    .WithSkip<Expectation>(exp => exp.Timestamp)
                    .WithSkip<Expectation>(exp => exp.ModifiedOn)
                    .WithSkip<Expectation>(exp => exp.CreatedOn);
                // Configures the generator overrides to use - can be called multiple times
            });
        }

        public static HttpResponse FakeResponse()
        {
            var resposeFaker = new AutoFaker<HttpResponse>()
                .RuleFor(exp => exp.Delay, fake => new Delay(fake.PickRandom<TimeUnit>(), fake.Random.Int(50, 100000)))
                .RuleFor(exp => exp.Body, FakeBody());

            return resposeFaker.Generate();
        }


        public static Expectation FakeExpectation() => ExpectationFaker.Generate();
        public static List<Expectation> FakeExpectations(int count) => ExpectationFaker.Generate(count);
        
        public static JToken FakeBody()
        {
            Func<IFluentBodyBuilder> factory = () => new FluentBodyBuilder();
            
            var faker = new AutoFaker<JToken>();
            var list = BodyOptions.Select(act =>
            {
                var builder = factory.Invoke();
                act(builder);
                return builder.Build();
            }).ToList();
            
            return faker.RuleFor( x => x, fake => fake.Random.Shuffle(list));
        }


        /// <summary>Gets a random <see cref="HttpRequest" />.</summary>
        /// <returns></returns>
        public static HttpRequest GetRandomHttpRequest() => HttpRequestFaker.Generate();
    } 
    public class ExpectationService
    {
        private readonly ILogger<ExpectationService> _logger;
        private readonly ExpectationDbContext _context;
        
        public ExpectationService(ExpectationDbContext context, ILogger<ExpectationService> logger)
        {
            _logger = logger;
            _context = context;
//            SeedAsync(5);
        }
        
        public async IAsyncEnumerable<Expectation> FindExpectationsAsync(Func<Expectation, bool> predicate, [EnumeratorCancellation] CancellationToken token = default)
        {
            await foreach (var expectation in _context.Expectations.AsAsyncEnumerable().WithCancellation(token))
            {
                if (predicate(expectation))
                {
                    yield return expectation;
                }
            }
        }

        public async Task<List<Expectation>> ToListAsync(CancellationToken token = default) => await _context.Expectations.ToListAsync(token);
        
        public async IAsyncEnumerable<Expectation> GetAllAsync([EnumeratorCancellation] CancellationToken token = default)
        {
            await foreach (var expectation in  _context.Expectations.AsAsyncEnumerable().WithCancellation(token))
            {
                yield return expectation;
            }
        }
        
        public async ValueTask<long> PruneAsync(CancellationToken token = default)
        {
            var count = await _context.Expectations.ClearAsync(token);
            await _context.SaveChangesAsync(token);
            _logger.LogWarning($"Pruned {count.ToString()} entries from DbSet<{nameof(Expectation)}>.");
            return count;
        }

        public async ValueTask<int?> AddAsync(Expectation expectation, CancellationToken token = default)
        {
            try
            {
                await _context.Expectations.AddAsync(expectation, token);
                await _context.SaveChangesAsync(token);
                return expectation.Id;
            }
            catch (Exception e)
            {
                _logger.LogError($"Could not save {nameof(Expectation)} to DbSet<{nameof(Expectation)}>.", e);
                return null;
            }
        }
        
        public async IAsyncEnumerable<Expectation> SeedAsync(int count, [EnumeratorCancellation] CancellationToken token = default)
        {
#if DEBUG
            var expectations = ExpectationFactory.FakeExpectations(count);
            await _context.Expectations.AddRangeAsync(expectations, token);
            await _context.SaveChangesAsync(token);

            await foreach (var expectation in _context.Expectations.AsAsyncEnumerable())
            {
                yield return expectation;
            }
#endif
        }
        
    }
}
