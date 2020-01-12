using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Models;
using NinjaTools.FluentMockServer.API.Proxy.Evaluation;
using NinjaTools.FluentMockServer.API.Proxy.Evaluation.Models;
using NinjaTools.FluentMockServer.API.Proxy.Evaluation.Visitors;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.API.Tests.Proxy.Matchers
{

    public class EvaluatorTestData : TheoryData<RequestMatcher, HttpContext, EvaluationResultBase>
    {

        protected static HttpContext GetContext(Action<HttpRequest> setup)
        {
            var context = new DefaultHttpContext();
            setup(context.Request);
            return context;
        }

        protected EvaluationResultBase Eval(int score, bool valid)
        {
            return new ResultProxy(valid, new EvaluationContext(new DefaultHttpContext()));
        }

        public class ResultProxy : EvaluationResultBase
{
    /// <inheritdoc />
    public ResultProxy(bool math, EvaluationContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public override bool IsMatch { get; }
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
                var eval = Eval(10, true);
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
                    Query =  "?id=0",
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
                var evalA = Eval(0, false);
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
                    Query = "?id=123"
                };
                var evalA = Eval(6, false);
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
        public void Should_Return_EvaluationUnsuccessfulResult_When_Not_Matching(RequestMatcher matcher, HttpContext context, EvaluationResultBase expectedResult)
        {
            // Act && Assert
            var visitor = new RequestEvaluatorVistor(new EvaluationContext(context));
            var result = visitor.Evaluate();
            Dump(matcher, "matcher");
            result.Should().As<EvaluationResultBase>().Should().Be(false);
            result.IsMatch.Should().BeFalse();
            result.Score.Should().Be(expectedResult.Score);
        }

        [Theory]
        [ClassData(typeof(EvaluatorTestData.ValidRequestMatcherTestData))]
        public void Should_Return_EvaluationSuccessfulResult_When_Matching(RequestMatcher matcher, HttpContext context, EvaluationResultBase expectedResult)
        {
            // Act && Assert
            var visitor = new RequestEvaluatorVistor(new EvaluationContext(context));
            var result = matcher.Accept(() => visitor);
            Dump(matcher, "matcher");
            result.Should().As<EvaluationResultBase>().Should().Be(false);
            result.IsMatch.Should().Be(true);
            result.Score.Should().Be(expectedResult.Score);
        }

        //
        // [Theory]
        // [MemberData(nameof(GetScoreTestData))]
        // public void Score_Should_Be_Higher_For_The_Matcher_Who_Is_Closer_To_The_HttpRequest(RequestMatcher more, HttpContext context, RequestMatcher less)
        // {
        //     // Act
        //     var a  = EvaluationPipeline.Evaluate(context, more);
        //     Dump(a, "more");
        //
        //     var b = EvaluationPipeline.Evaluate(context, less);
        //     Dump(b, "less");
        //
        //     // Assert
        //     a.Score.Should().BeGreaterOrEqualTo(b.Score);
        // }

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
                    less.Query = "?id=0";
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
