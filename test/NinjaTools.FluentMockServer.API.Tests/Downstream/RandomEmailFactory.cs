using System.Collections.Generic;
using Bogus;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace NinjaTools.FluentMockServer.API.Tests.Downstream
{
    public static class RandomEmailFactory
    {
        public const int Seed = 100;
        public static readonly Faker<Email> EmailFaker;

        static RandomEmailFactory()
        {
            EmailFaker = new Faker<Email>()
                .UseSeed(Seed)
                .RuleFor(mail => mail.From, f => f.Internet.Email(f.Name.FirstName(), f.Name.LastName()))
                .RuleFor(mail => mail.To, f => f.Internet.ExampleEmail("aheld"))
                .RuleFor(mail => mail.Content, f => JsonConvert.SerializeObject(f.Rant.Reviews(f.Commerce.ProductName(), f.Random.Int(1, 5))))
                .RuleFor(mail => mail.Subject, f => f.Company.CatchPhrase());
            
            EmailFaker.AssertConfigurationIsValid();
        }


        /// <summary>
        /// Generates one pseudo random generated <see cref="Email"/> instances.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public static Email GenerateOneRandomEmail() => EmailFaker.Generate();

        /// <summary>
        /// Generates <see cref="count"/> of pseudo random generated <see cref="Email"/> instances.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<Email> Generate(int count) => EmailFaker.Generate(count);
    }
}
