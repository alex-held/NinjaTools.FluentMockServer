// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using System.IO;
// using JetBrains.Annotations;
// using Microsoft.AspNetCore.Http;
// using Newtonsoft.Json;
// using Newtonsoft.Json.Converters;
// using NinjaTools.FluentMockServer.API.Proxy.Evaluation.Visitors;
//
// namespace NinjaTools.FluentMockServer.API.Models
// {
//     public class RequestBodyMatcher
//     {
//         public string? Content { get; set; }
//         public RequestMatcher.RequestBodyKind  Type { get; set; }
//         public bool MatchExact { get; set; }
//     }
//
//     public class RequestMatcher
//     {
//         private string? _path;
//         public string? Path
//         {
//             get => _path;
//             set => _path = value is null
//                 ? null
//                 : $"/{value.TrimStart('/')}";
//         }
//
//         public RequestBodyMatcher? BodyMatcher { get; set; }
//         public string Method { get; set; }
//         public Dictionary<string, string[]>? Headers { get; set; }
//         public Dictionary<string, string>? Cookies { get; set; }
//         public string? Query { get; set; }
//
//
//         [JsonConverter(typeof(StringEnumConverter))]
//         public enum RequestBodyKind
//         {
//             Text,
//             Base64
//         }
//     }
// }
