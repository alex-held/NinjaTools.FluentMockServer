using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Helper;
using NinjaTools.FluentMockServer.API.Models;
using NinjaTools.FluentMockServer.API.Models.Logging;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;
using HttpResponse = NinjaTools.FluentMockServer.API.Models.HttpResponse;

namespace NinjaTools.FluentMockServer.API.Tests.Models.Logging
{
    public class LogItemTestData : TheoryData<ILogItem,  string>
    {
        public static HttpContext Context = new DefaultHttpContext()
        {
            Request = { Path = "/request/path",Method = "PUT" }
        };

        public static readonly Setup DefaultSetup = new Setup
        {
            Action = new ResponseAction
            {
                Response = new HttpResponse
                {
                    Body = "body",
                    StatusCode = 200
                }
            },
            Matcher = new RequestMatcher
            {
                Method = "POST",
                Path = "/test/path"
            }
        };

        internal static readonly SetupLog SetupCreated;
        internal static readonly SetupLog SetupDeleted;
        internal static readonly RequestUnmatchedLog RequestUnmatched;
        internal static readonly RequestMatchedLog RequestMatched;

        static LogItemTestData()
        {
            var factory = new LogFactory();
            SetupCreated = factory.SetupCreated(DefaultSetup);
            SetupDeleted = factory.SetupDeleted(DefaultSetup);
            RequestMatched = factory.RequestMached(Context, DefaultSetup);
            RequestUnmatched = factory.RequestUnmatched(Context);
        }

        private void AddLogItem(ILogItem logItem, string expected)
        {
            Add(logItem, expected.Replace("{id}", logItem.Id));
        }

        public LogItemTestData()
        {
            AddLogItem(SetupCreated, @"Setup created! Id={id};
{
  ""Matcher"": {
    ""Path"": ""/test/path"",
    ""Method"": ""POST""
  },
  ""Action"": {
    ""Response"": {
      ""StatusCode"": 200,
      ""Body"": ""body""
    }
  }
}
");
            AddLogItem(SetupDeleted, @"Setup deleted! Id={id};
{
  ""Matcher"": {
    ""Path"": ""/test/path"",
    ""Method"": ""POST""
  },
  ""Action"": {
    ""Response"": {
      ""StatusCode"": 200,
      ""Body"": ""body""
    }
  }
}
");
            AddLogItem(RequestMatched, @"Matched PUT /request/path Id={id};
{
  ""HttpRequest"": {
    ""Method"": ""PUT"",
    ""Path"": ""/request/path"",
    ""IsHttps"": false
  },
  ""Setup"": {
    ""Matcher"": {
      ""Path"": ""/test/path"",
      ""Method"": ""POST""
    },
    ""Action"": {
      ""Response"": {
        ""StatusCode"": 200,
        ""Body"": ""body""
      }
    }
  }
}
");
            AddLogItem(RequestUnmatched, @"Unmatched PUT /request/path Id={id};
{
  ""Method"": ""PUT"",
  ""Path"": ""/request/path"",
  ""IsHttps"": false
}
");
        }
    }

    public class LogItemTests : XUnitTestBase<LogItemTests>
    {

        /// <inheritdoc />
        public LogItemTests(ITestOutputHelper output) : base(output)
        { }

        [Fact]
        public void SetupLog_LogType_Should_Return_Setup()
        {
            // Arrange
            ILogItem log = LogItemTestData.SetupCreated;

            // Act & Assert
            log.Type.Should().Be(LogType.Setup);
        }


        [Fact]
        public void RequestMatchedLog_LogType_Should_Return_Request()
        {
            // Arrange
            ILogItem log = LogItemTestData.RequestMatched;

            // Act & Assert
            log.Type.Should().Be(LogType.Request);
        }


        [Fact]
        public void RequestUnmatchedLog_LogType_Should_Return_Request()
        {
            // Arrange
            ILogItem log = LogItemTestData.RequestUnmatched;

            // Act & Assert
            log.Type.Should().Be(LogType.Request);
        }

        [Theory]
        [ClassData(typeof(LogItemTestData))]
        public void ToFormattedString_Should_Return_PrettyFormatted_String(ILogItem log, string expected)
        {
            // Act
            var formatted = log.ToFormattedString();

            Dump(expected, formatted);
            
            // Assert
            formatted.Should().Be(expected);
        }
    }
}
