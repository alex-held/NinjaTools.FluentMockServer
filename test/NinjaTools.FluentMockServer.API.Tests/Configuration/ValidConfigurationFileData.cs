using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Configuration;
using NinjaTools.FluentMockServer.API.Models;
using Xunit;
using HttpResponse = NinjaTools.FluentMockServer.API.Models.HttpResponse;

namespace NinjaTools.FluentMockServer.API.Tests.Configuration
{
    public class ValidConfigurationFileData : TheoryData<IConfigurationFile, string, string>
    {
        public ValidConfigurationFileData()
        {
            Add(new ConfigurationFile()
                {
                    new Setup
                    {
                        Matcher = new RequestMatcher()
                        {
                            Headers = new Dictionary<string, string[]>
                            {
                                {"Content-Type", new[] {"application/json", "application/cloudevents+json"}},
                                {"Content-Length", new[] {"105"}}
                            },
                            Path = "/some/path",
                            Method = "POST",
                            BodyMatcher = new RequestBodyMatcher
                            {
                                Content = "request body to match \n over \n multiple \nlines",
                                MatchExact = false,
                                Type = RequestBodyType.Text
                            },
                            QueryString = new QueryString("?id=123")
                        },
                        Action = new ResponseAction
                        {
                            Response = new HttpResponse
                            {
                                Body = "some body \n over \n multiple \nlines",
                                StatusCode = 200
                            }
                        }
                    }
                },
                @"[
  {
    ""Id"": ""1"",
    ""Matcher"": {
      ""BodyMatcher"": {
        ""Content"": ""request body to match \n over \n multiple \nlines"",
        ""Type"": 0,
        ""MatchExact"": false
      },
      ""Path"": ""/some/path"",
      ""Method"": ""POST"",
      ""Headers"": {
        ""Content-Type"": [
          ""application/json"",
          ""application/cloudevents+json""
        ],
        ""Content-Length"": [
          ""105""
        ]
      },
      ""QueryString"": {
        ""Value"": ""?id=123"",
        ""HasValue"": true
      }
    },
    ""Action"": {
      ""Response"": {
        ""StatusCode"": 200,
        ""Body"": ""some body \n over \n multiple \nlines""
      }
    }
  }
]", @"- Id: 1
  Matcher:
    BodyMatcher:
      Content: ""request body to match \n over \n multiple \nlines""
      Type: Text
      MatchExact: false
    Path: /some/path
    Method: POST
    Headers:
      Content-Type:
      - application/json
      - application/cloudevents+json
      Content-Length:
      - 105
    QueryString:
      Value: ?id=123
      HasValue: true
  Action:
    Response:
      StatusCode: 200
      Body: ""some body \n over \n multiple \nlines""");
        }

        public const int ConfigurationFileIndex = 0;
        public const int YamlIndex = 2;
        public const int JsonIndex = 1;
    }
}
