// using AutoMapper;
// using Microsoft.AspNetCore.Http;
// using NinjaTools.FluentMockServer.API.Models;
// using NinjaTools.FluentMockServer.API.Proxy.Evaluation.Evaluators;
// using NinjaTools.FluentMockServer.API.Proxy.Evaluation.Models;
//
// namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation
// {
//     public static class EvaluationPipeline
//     {
//         private static readonly IEvaluator Pipeline;
//
//         static EvaluationPipeline()
//         {
//             var evaluator = new HttpRequestEvaluator();
//             evaluator.SetNext(new HttpPathEvaluator())
//                 .SetNext(new HttpMethodEvaluator())
//                 .SetNext(new HttpBodyEvaluator())
//                 .SetNext(new HttpCookieEvaluator())
//                 .SetNext(new QueryStringEvaluator())
//                 .SetNext(new HttpHeaderEvaluator());
//
//             Pipeline = evaluator;
//         }
//
//         public static IEvaluationResult Evaluate(HttpContext httpContext, RequestMatcher matcher)
//         {
//             var context = new EvaluationContext(httpContext, matcher.Normalize());
//             var evalResult = Pipeline.Evaluate(context);
//             return evalResult;
//         }
//
//
//
//
//
//         private static IMapper? _mapper;
//         public static IMapper Mapper
//         {
//             get
//             {
//                 if (_mapper is null)
//                 {
//                     var config = new MapperConfiguration(cfg => cfg.AddProfile<RequestMatcherProfile>());
//                     _mapper = config.CreateMapper();
//                 }
//
//                 return _mapper;
//             }
//         }
//
//         public static NormalizedMatcher Normalize(this RequestMatcher matcher)
//         {
//             var result =  Mapper.Map<NormalizedMatcher>(matcher);
//             return result;
//         }
//
//         public class RequestMatcherProfile : Profile
//         {
//             public RequestMatcherProfile()
//             {
//                 ShouldMapField = _ => true;
//                 ShouldMapProperty = _ => true;
//                 EnableNullPropagationForQueryMapping = true;
//                 AllowNullCollections = true;
//                 AllowNullDestinationValues = true;
//
//                 CreateMap<RequestMatcher, NormalizedMatcher>()
//                     .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.BodyMatcher.Content))
//                     .ForMember(dest => dest.MatchExact, opt => opt.MapFrom(src => src.BodyMatcher.MatchExact))
//                     .ForMember(dest => dest.BodyKind, opt => opt.MapFrom(src => src.BodyMatcher.Type))
//                     .ForMember(dest => dest.Query, opt => opt.MapFrom(src => src.Query))
//                     .ReverseMap();
//             }
//
//
//         }
//     }
// }
