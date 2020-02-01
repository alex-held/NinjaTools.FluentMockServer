using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Moq;
using NinjaTools.FluentMockServer.API.Infrastructure;
using NinjaTools.FluentMockServer.API.Logging;
using NinjaTools.FluentMockServer.API.Logging.Models;
using NinjaTools.FluentMockServer.API.Models;
using NinjaTools.FluentMockServer.API.Services;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;
using HttpRequest = NinjaTools.FluentMockServer.Models.HttpEntities.HttpRequest;
using Path = NinjaTools.FluentMockServer.API.Models.Path;

namespace NinjaTools.FluentMockServer.API.Tests.Infrastructure
{
    public class SetupRepositoryTests : XUnitTestBase<SetupRepositoryTests>
    {
        /// <inheritdoc />
        public SetupRepositoryTests(ITestOutputHelper output) : base(output)
        { }

        private ISetupRepository CreateSubject(out Mock<ILogService> logService)
        {
            logService = new Mock<ILogService>();
            return new SetupRepository(logService.Object);
        }

        private ISetupRepository CreateSubject(params Setup[] setups)
        {
            var subject =  new SetupRepository(Mock.Of<ILogService>());
            foreach (var setup in setups)
            {
                subject.Add(setup);
            }

            return subject;
        }

        [Fact]
        public void Add_Should_Create_A_SetupLogCreated_Log_Entry()
        {
            // Arrange
            var sut = CreateSubject(out var logService);
            var setup = new Setup(new RequestMatcher
            {
                Path = new Path("/some/path")
            });

            // Act
            sut.Add(setup);

            // Assert
            logService.Verify(m => m.Log(It.IsAny<Func<LogFactory, LogItem<Setup>>>()));
        }

        [Fact]
        public void TryGetMatchingSetup_Returns_Setup_With_Most_Fullfilled_Constrains()
        {
            // Arrange
            var (expectedTopMatcher, otherMatchers) = CreateTopMatcher();
            var context = Map(expectedTopMatcher);
            var setups = otherMatchers.Select(rm => new Setup(rm)).ToArray();
            var sut = CreateSubject(setups);

            // Act
            var matchingSetup = sut.TryGetMatchingSetup(context);

            // Assert
            matchingSetup.Matcher.Should().Be(expectedTopMatcher);
        }

        [Fact]
        public void TryGetMatchingSetup_Returns_Valid_Setup_Even_If_Less_Contrains_Fullfilled()
        {
            // Arrange
            var expectedTopMatch = new RequestMatcher
            {
                Path = new Path("/only/this/path/is/correct/but/the/other/setups/dont/match"),
                Query = new Query("?id=123"),
                Headers = new Headers
                {
                    {"Content-Type", new []{"application/json"}}
                }
            };

            var setups = new[]
            {
                new Setup(new RequestMatcher
                {
                    Path = new Path("/only/this/path/is/correct/but/the/other/setups/dont/match")
                }),
                new Setup(new RequestMatcher
                {
                    Path = new Path("/this/path/is/wrong/thats/why/it/cant/match"),
                    Method = new Method("GET"),
                    Query = new Query("?id=123")
                }),
                new Setup(new RequestMatcher
                {
                    Path = new Path("/this/path/is/wrong/thats/why/it/cant/match"),
                    Query = new Query("?id=123"),
                    Headers = new Headers
                    {
                        {"Content-Type", new []{"application/json"}}
                    }
                })
            };

            var context = Map(expectedTopMatch);
            var sut = CreateSubject(setups);

            // Act
            var setup = sut.TryGetMatchingSetup(context);


            // Assert
            setup.Matcher.Path.ToPath().Should().Be("/only/this/path/is/correct/but/the/other/setups/dont/match");
        }

        private static (RequestMatcher expectedTopMatcher, List<RequestMatcher> otherMatchers) CreateTopMatcher()
        {
            var expectedTopMatcher = new RequestMatcher
            {
                Path = new Path("/some/path"),
                Method = new Method("GET"),
                Query = new Query("?id=123")
            };

            var otherMatchers = new List<RequestMatcher>
            {
                new RequestMatcher
                {
                    Path = new Path("/some/path"),
                },
                new RequestMatcher
                {
                    Method = new Method("GET"),
                    Query = new Query("?id=123")
                },

                new RequestMatcher
                {
                    Path = new Path("/some/path"),
                    Query = new Query("?id=123")
                },

                new RequestMatcher
                {
                    Path = new Path("/some/path"),
                    Method = new Method("GET"),
                    Query = new Query("?id=1000")
                },
                new RequestMatcher
                {
                    Path = new Path("/nope"),
                    Method = new Method("GET"),
                    Query = new Query("?id=1000")
                },
                expectedTopMatcher,
                new RequestMatcher
                {
                    Path = new Path("/some/path"),
                    Method = new Method("POST"),
                    Query = new Query("?id=123")
                },
                new RequestMatcher
                {
                    Path = new Path("/b/a"),
                    Method = new Method("PATCH")
                },
                new RequestMatcher
                {
                    Path = new Path("/xxx"),
                    Method = new Method("ERROR"),
                    Query = new Query("?id=666")
                },
            };
            return (expectedTopMatcher, otherMatchers);
        }

        public HttpContext Map(RequestMatcher requestMatcher)
        {
            var context = new DefaultHttpContext
            {
                Request =
                {
                    Cookies = requestMatcher.Cookies,
                    Method = requestMatcher.Method,
                    QueryString = requestMatcher.Query.QueryString,
                    Path = requestMatcher.Path.PathString
                }
            };

            if (requestMatcher.TryGetBodyString(out var bodyString))
            {
                var bodyStream = new MemoryStream();
                using var streamWriter = new StreamWriter(bodyStream, Encoding.Default);
                streamWriter.Write(bodyString);
                context.Request.Body = bodyStream;
            }

            if (requestMatcher.TryGetHeaders(out var headers))
            {
                foreach (var header in headers)
                {
                    context.Request.Headers.TryAdd(header.Key, header.Value);
                }
            }

            return context;

        }
    }
}
