using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NinjaTools.FluentMockServer.API.Models;
using HttpResponse = NinjaTools.FluentMockServer.API.Models.HttpResponse;

namespace NinjaTools.FluentMockServer.API.Controllers
{
    /// <inheritdoc />
    public class SetupViewModelProfile : Profile
    {
        /// <inheritdoc />
        public SetupViewModelProfile()
        {
            CreateMap<Setup, SetupViewModel>()
                .ForMember(dest => dest.Matcher, o => o.MapFrom(src => src.Matcher))
                .ForMember(dest => dest.Action, o => o.MapFrom(src => src.Action))
                .ReverseMap();

            CreateMap<RequestMatcher, RequestMatcherViewModel>()
                .ForMember(dest => dest.Path, o => o.MapFrom(src => src.Path.PathString.Value))
                .ForMember(dest => dest.Method, o => o.MapFrom(src => src.Method.MethodString))
                .ForMember(dest => dest.Cookies, o => o.MapFrom(src => src.Cookies.ToDictionary(k => k.Key, v => v.Value)))
                .ForMember(dest => dest.Headers, o => o.MapFrom(src => src.Headers.ToDictionary(k => k.Key, v => v.Value)))
                .ForMember(dest => dest.Query, o => o.MapFrom(src => src.Query.QueryString.Value))
                .ReverseMap()
                .ForMember(dest => dest.Path, o => o.MapFrom(src => new Path(src.Path)))
                .ForMember(dest => dest.Query, o => o.MapFrom(src => new Query(src.Query)))
                .ForMember(dest => dest.Method, o => o.MapFrom(src => new Method(src.Method)))
                .ForMember(dest => dest.Headers, o => o.MapFrom(src => new Headers(src.Headers)))
                .ForMember(dest => dest.Cookies, o => o.MapFrom(src => src == null ? null : new Cookies(src.Cookies) ));


            CreateMap<ResponseActionViewModel, ResponseActionViewModel>()
                .ForMember(dest => dest.Response, o => o.MapFrom(src => src.Response))
                .ReverseMap();

            CreateMap<RequestBodyMatcher, RequestBodyMatcherViewModel>()
                .ForMember(dest => dest.Content, o => o.MapFrom(src => src.Content))
                .ForMember(dest => dest.MatchExact, o => o.MapFrom(src => src.MatchExact))
                .ForMember(dest => dest.Kind, o => o.MapFrom(src => src.Kind))
                .ReverseMap();

            CreateMap<HttpResponse, HttpResponseViewModel>()
                .ForMember(dest => dest.Body, o => o.MapFrom(src => src.Body))
                .ForMember(dest => dest.StatusCode, o => o.MapFrom(src => src.StatusCode))
                .ReverseMap();
        }
    }

    /// <summary>
    /// Defines how the MockServer responds zu a matched requests.
    /// </summary>
    public class ResponseActionViewModel
    {
        public HttpResponseViewModel? Response { get; set; }
    }

    public class HttpResponseViewModel
    {
        public int? StatusCode { get; set; }
        public string? Body { get; set; }
    }

    public class SetupViewModel
    {
        public RequestMatcherViewModel? Matcher { get; set; }

        public ResponseAction? Action { get; set; }
    }

    public class RequestMatcherViewModel
    {
        public string? Method { get;set; }
        public string? Path { get; set;}
        public string? Query { get; set;}
        public bool? IsHttps { get; set;}
        public Dictionary<string, string[]>? Headers { get;set;}
        public Dictionary<string, string>? Cookies { get; set;}
        public RequestBodyMatcherViewModel? Body { get; set; }
    }


    public class RequestBodyMatcherViewModel
    {
        public string? Content { get; set; }

        public bool? MatchExact { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RequestBodyKind?  Kind { get; set; }
    }

}
