using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Moq;
using NinjaTools.FluentMockServer.API.Models;
using NinjaTools.FluentMockServer.API.Proxy.Evaluation;
using NinjaTools.FluentMockServer.API.Proxy.Evaluation.Models;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.API.Tests.Proxy.Matchers
{

    public class EvaluatorTestData : TheoryData<RequestMatcher, HttpContext, IEvaluationResult>
    {

        protected static HttpContext GetContext(Action<HttpRequest> setup)
        {
            var context = new DefaultHttpContext();
            setup(context.Request);
            return context;
        }

        protected IEvaluationResult Eval(int score, [CanBeNull] EvaluationMessages messages = null)
        {
            messages ??= new EvaluationMessages();
            return Mock.Of<IEvaluationResult>(e => e.Score == score
                                                   && e.IsMatch == false
                                                   && e.Messages == messages.Messages
                                                   && e.Errors == messages.Exceptions);
        }




        public class ValidRequestMatcherTestData : EvaluatorTestData
        {
            public ValidRequestMatcherTestData()
            {
                Data1();
            }

            private void Data1()
            {
                // *should* match anything
                var matcher = new RequestMatcher();
                var context = GetContext(r =>
                {
                    r.Method = HttpMethods.Delete;
                    r.Path = "/cars/buy/id/200";
                    r.ContentType = "application/json";
                });
                var eval = Eval(10);
                Add(matcher, context, eval);
            }
        }


        public class InvalidRequestMatcherTestData : EvaluatorTestData
        {
            public InvalidRequestMatcherTestData()
            {
                Data1();
                Data2();
            }

            private void Data1()
            {
                var reqA = new RequestMatcher
                {
                    Path = "/some/path",
                    Cookies = new Dictionary<string, string>
                    {
                        {"abcd", "application/json"}
                    },
                    QueryString =  "?id=0",
                    Method = "GET",
                    Headers = new Dictionary<string, string[]>
                    {
                        { "a", new [] {""}}
                    },
                    BodyMatcher = new RequestBodyMatcher
                    {
                        Content = "?"
                    }
                };
                var evalA = Eval(0);
                var ctxA = GetContext(r =>
                {
                    r.Method = HttpMethods.Delete;

                });
                Add(reqA, ctxA, evalA);
            }

            private void Data2()
            {
                var reqA = new RequestMatcher
                {
                    Path = "/x&/y(",
                    QueryString = "?id=123"
                };
                var evalA = Eval(6);
                var ctxA = GetContext(r => { });

                Add(reqA, ctxA, evalA);
            }
        }
    }



    public class EvaluatorTests: XUnitTestBase<EvaluatorTests>
    {
        /// <inheritdoc />
        public EvaluatorTests(ITestOutputHelper output) : base(output)
        { }

        [Theory]
        [ClassData(typeof(EvaluatorTestData.InvalidRequestMatcherTestData))]
        public void Should_Return_EvaluationUnsuccessfulResult_When_Not_Matching(RequestMatcher matcher, HttpContext context, IEvaluationResult expectedResult)
        {
            // Act && Assert

            var result  = EvaluationPipeline.Evaluate(context, matcher);
            Dump(matcher, "matcher");
            result.Should().BeOfType<EvaluationUnsuccessfulResult>();
            result.IsMatch.Should().BeFalse();
            result.Score.Should().Be(expectedResult.Score);
        }

        [Theory]
        [ClassData(typeof(EvaluatorTestData.ValidRequestMatcherTestData))]
        public void Should_Return_EvaluationSuccessfulResult_When_Matching(RequestMatcher matcher, HttpContext context, IEvaluationResult expectedResult)
        {
            // Act && Assert
            var result  = EvaluationPipeline.Evaluate(context, matcher);
            Dump(matcher, "matcher");
            result.Should().BeOfType<EvaluationSuccessfulResult>();
            result.IsMatch.Should().Be(true);
            result.Score.Should().Be(expectedResult.Score);
        }


        [Theory]
        [MemberData(nameof(GetScoreTestData))]
        public void Score_Should_Be_Higher_For_The_Matcher_Who_Is_Closer_To_The_HttpRequest(RequestMatcher more, HttpContext context, RequestMatcher less)
        {
            // Act
            var a  = EvaluationPipeline.Evaluate(context, more);
            Dump(a, "more");

            var b = EvaluationPipeline.Evaluate(context, less);
            Dump(b, "less");

            // Assert
            a.Score.Should().BeGreaterOrEqualTo(b.Score);
        }

        public static IEnumerable<object[]> GetScoreTestData()
        {
            yield return Build(req =>
                {
                    req.Path = "/a/b";
                    req.Method = "POST";
                },
                less => less.Path = "z",
                ctx =>
                {
                    ctx.Request.Path = PathString.FromUriComponent("/a/b");
                    ctx.Request.Method = "POST";
                });

            yield return Build(req =>
                {
                    req.Path = "/a/b";
                    req.Method = "POST";
                    req.BodyMatcher = new RequestBodyMatcher
                    {
                        Content = "?",
                        MatchExact = true
                    };
                },
                less =>
                {
                    less.Path = "/some/path";
                    less.Cookies = new Dictionary<string, string>
                    {
                        {"abcd", "application/json"}
                    };
                    less.QueryString = "?id=0";
                    less.Method = "GET";
                    less.Headers = new Dictionary<string, string[]>
                    {
                        {"a", new[] {""}}
                    };
                },
                ctx =>
                {
                    ctx.Request.Cookies.Append(new KeyValuePair<string, string>("abcd", "application/json"));
                    ctx.Request.Path = PathString.FromUriComponent("/a/b");
                    ctx.Request.Method = "POST";
                });

            yield return Build(req =>
                {
                    req.BodyMatcher = new RequestBodyMatcher
                    {
                        Content = "hello world",
                        MatchExact = true,
                        Type = RequestBodyKind.Text
                    };
                },
                less => less.Path = "/a/b/*",
                ctx =>
                {
                    ctx.Request.Path = PathString.FromUriComponent("/a/b");
                    ctx.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes("hello world"));
                });

            yield return Build(req => req.Path = "/a/b/*",
                less => less.Path = "/a/b",
                ctx =>
                {
                    ctx.Request.Path = PathString.FromUriComponent("/a/b");
                    ctx.Request.Method = "POST";
                    ctx.Request.Cookies.Append(new KeyValuePair<string, string>("abcd", "application/json"));
                    ctx.Request.Headers.Add("a", new[] {""});
                });
        }







        private static object[] Build(Action<RequestMatcher> factory, Action<RequestMatcher> lessAction, Action<HttpContext> ctxFac)
        {
            var ctx = new DefaultHttpContext();
            ctxFac(ctx);
            var more = new RequestMatcher();
            var less = new RequestMatcher();
            factory(more);
            factory(less);
            lessAction(less);
            return new object[] {more, ctx, less };
        }
    }
}
