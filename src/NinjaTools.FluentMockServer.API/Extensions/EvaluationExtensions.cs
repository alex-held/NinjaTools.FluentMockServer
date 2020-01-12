using System.Linq;
using NinjaTools.FluentMockServer.API.Proxy.Evaluation;
using NinjaTools.FluentMockServer.API.Proxy.Evaluation.Evaluators;
using NinjaTools.FluentMockServer.API.Proxy.Evaluation.Models;

namespace NinjaTools.FluentMockServer.API.Extensions
{
    public static class EvaluationExtensions
    {
        public static void AddExtraPoints(this EvaluationContext context, int bonus) => context.Ratings.BonusPoints += bonus;


        public static THttpRequestMember? EnsureNotNull<THttpRequestMember, TMockMember>(this EvaluationContext context,THttpRequestMember? httpRequestMember, TMockMember? item)
            where TMockMember : class
            where  THttpRequestMember : class
        {
            if (item is null && httpRequestMember is  null)
            {
                var rating = new Rating(1, EvaluationWeight.Low)
                {
                    HttpRequestMember =  new { Caller = typeof(THttpRequestMember).Name, Value ="<null>"},
                    Member = new { Caller = typeof(TMockMember).Name, Value ="<null>"}
                };

                context.Ratings.Append(rating);
                return null;
            }

            return httpRequestMember;
        }

        public static void Match<T>(this EvaluationContext context,  T? httpRequestMember, object? matcherMember)where T : class
        {
            var rating = new Rating(1, EvaluationWeight.Low)
            {
                HttpRequestMember = httpRequestMember,
                Member = matcherMember
            };

            context.Ratings.Append(rating);
        }


        public static void Fail<T>(this EvaluationContext context,  T? httpRequestMember, object? matcherMember) where T : class
        {
            var rating = new Rating(0, EvaluationWeight.Non)
            {
                HttpRequestMember = httpRequestMember,
                Member = matcherMember
            };

            context.Ratings.Append(rating);
        }
    }
}
