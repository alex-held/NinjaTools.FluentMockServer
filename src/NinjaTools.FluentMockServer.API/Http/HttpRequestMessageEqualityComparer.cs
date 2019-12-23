using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace NinjaTools.FluentMockServer.API.Http
{

    // internal static class BufferingHelper
    // {
    //     internal const int DefaultBufferThreshold = 30720;
    //
    //     [NotNull]
    //     public static HttpRequest EnableRewind(
    //         [NotNull] this HttpRequest request,
    //         int bufferThreshold = 30720,
    //         long? bufferLimit = null)
    //     {
    //         if (request == null)
    //             throw new ArgumentNullException(nameof(request));
    //         var body = request.Body;
    //         if (!body.CanSeek)
    //         {
    //             var bufferingReadStream = new FileBufferingReadStream(body, bufferThreshold, bufferLimit, Path.GetTempPath);
    //             request.Body = bufferingReadStream;
    //             request.HttpContext.Response.RegisterForDispose(bufferingReadStream);
    //         }
    //
    //         return request;
    //     }
    //
    //     public static MultipartSection EnableRewind(
    //         [NotNull] this MultipartSection section,
    //         [NotNull] Action<IDisposable> registerForDispose,
    //         int bufferThreshold = 30720,
    //         long? bufferLimit = null)
    //     {
    //         if (section == null)
    //             throw new ArgumentNullException(nameof(section));
    //         if (registerForDispose == null)
    //             throw new ArgumentNullException(nameof(registerForDispose));
    //         var body = section.Body;
    //         if (!body.CanSeek)
    //         {
    //             var bufferingReadStream = new FileBufferingReadStream(body, bufferThreshold, bufferLimit, AspNetCoreTempDirectory.TempDirectoryFactory);
    //             section.Body = (Stream) bufferingReadStream;
    //             registerForDispose((IDisposable) bufferingReadStream);
    //         }
    //
    //         return section;
    //     }
    // }
    //
    // public sealed class HttpRequestWrapper : HttpRequest
    // {
    //     [NotNull]
    //     private readonly HttpRequest _request;
    //
    //     public HttpRequestWrapper([NotNull] HttpRequest request)
    //     {
    //         _request = request;
    //         Body = request.Body;
    //         Cookies = request.Cookies,
    //         ContentLength = request.ContentLength,
    //         PathBase = request.PathBase,
    //         Form = request.Form,
    //         Host = request.Host,
    //         Protocol = request.Protocol,
    //         Query = request.Query,
    //         QueryString = request.QueryString,
    //         Scheme = request.Scheme,
    //         Path = request.Path,
    //         HttpContext = request.HttpContext,
    //         IsHttps = request.IsHttps,
    //         Method = request.Method,
    //         ContentType = request.ContentType,
    //         RouteValues = request.RouteValues
    //     }
    //     
    //     
    //     /// <inheritdoc />
    //     public HttpRequestWrapper()
    //     {
    //         if (Body !=  null && Body.GetType() == Stream.Null.GetType())
    //         {
    //             Body = null;
    //         }
    //     }
    //
    //     /// <inheritdoc />
    //     public override Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = new CancellationToken())
    //     {
    //         return new FormFeature(Form).ReadFormAsync(cancellationToken);
    //     }
    //     
    //
    //     /// <inheritdoc />
    //     [JsonIgnore]
    //     public sealed override Stream Body { get; set; }
    //
    //     /// <summary>
    //     /// The <see cref="Stream"/> Body.
    //     /// </summary>
    //     [JsonProperty("Body")]
    //     [CanBeNull]
    //     public string BodyContent
    //     {
    //         get
    //         {
    //             if (Body != null)
    //             {
    //                 var ms = new MemoryStream();
    //                 Body.CopyTo(ms);
    //                 return Convert.FromBase64CharArray(Encoding.UTF8);
    //                 var sr = new StreamReader(Body, Encoding.UTF8, true, 1024, true);
    //                 var bodyContent = sr.ReadToEnd();
    //                 Body.Position = 0;
    //
    //             }
    //             return null;
    //         }
    //     }
    //
    //     /// <inheritdoc />
    //     public override long? ContentLength { get; set; }
    //
    //     /// <inheritdoc />
    //     public override string ContentType { get; set; }
    //
    //     /// <inheritdoc />
    //     public override IRequestCookieCollection Cookies { get; set; }
    //
    //     /// <inheritdoc />
    //     public override IFormCollection Form { get; set; }
    //
    //     /// <inheritdoc />
    //     public override bool HasFormContentType => new FormFeature(this).HasFormContentType;
    //     
    //     /// <inheritdoc />
    //     public override IHeaderDictionary Headers { get; }
    //
    //     /// <inheritdoc />
    //     public override HostString Host { get; set; }
    //
    //     /// <inheritdoc />
    //     public override HttpContext HttpContext { get; }
    //
    //     /// <inheritdoc />
    //     public override bool IsHttps { get; set; }
    //
    //     /// <inheritdoc />
    //     public sealed override string Method { get; set; }
    //
    //     /// <inheritdoc />
    //     public override PathString Path { get; set; }
    //
    //     /// <inheritdoc />
    //     public override PathString PathBase { get; set; }
    //
    //     /// <inheritdoc />
    //     public override string Protocol { get; set; }
    //
    //     /// <inheritdoc />
    //     public override IQueryCollection Query { get; set; }
    //
    //     /// <inheritdoc />
    //     public override QueryString QueryString { get; set; }
    //
    //     /// <inheritdoc />
    //     public sealed override string Scheme { get; set; }
    // }
    //
    //
    //
    public class HttpRequestMessageEqualityComparer : IEqualityComparer<HttpRequestMessage>
    {
        public bool Equals(HttpRequestMessage x, HttpRequestMessage y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return Equals(x.Method, y.Method) && Equals(x.RequestUri, y.RequestUri) && Equals(x.Headers, y.Headers) && Equals(x.Version, y.Version) && Equals(x.Content, y.Content)  && Equals(x.Properties, y.Properties);
        }

        public int GetHashCode(HttpRequestMessage obj)
        {
            return HashCode.Combine( obj.Method, obj.RequestUri, obj.Headers, obj.Version, obj.Content, obj.Properties);
        }
            
        public static IEqualityComparer<HttpRequestMessage> Default { get; } = new HttpRequestMessageEqualityComparer();

    }
}
