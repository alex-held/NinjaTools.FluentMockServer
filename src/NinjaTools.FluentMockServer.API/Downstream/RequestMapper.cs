using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using NinjaTools.FluentMockServer.API.Configuration;
using NinjaTools.FluentMockServer.API.Extensions.Responses;
using NinjaTools.FluentMockServer.API.Http;
using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;

namespace NinjaTools.FluentMockServer.API.Downstream
{
    public class RequestMappingError : Error
    {
        /// <inheritdoc />
        public RequestMappingError([NotNull] Exception exception) : base(exception.Message, MockServerErrorCode.RequestMappingError)
        {
        }
    }

    /// <summary>
    /// Maps a <typeparam name="T_ =>"/> to a <typeparam name="TTo" />.
    /// </summary>
    /// <typeparam name="T_ =>"/>
    /// <typeparam name="TTo"/>
    public interface IMapper<in TFrom, TTo>
    {
        /// <summary>
        /// TODO: Not store in IMapper -> contradicts  SRP
        /// </summary>
        Func<TTo, TTo, bool> EqualityComparerDelegate { get; }

        /// <summary>
        /// Maps the <paramref name="from"/> to a <typeparam name="TTo" />.
        /// </summary>
        /// <param name="from">The <see cref="TFrom"/>.</param>
        /// <returns>
        /// Returns a <see cref="Response{TTo}"/> containing the mapping result or any <see cref="Error"/> that occured.
        /// </returns>
        Task<Response<TTo>> MapAsync(TFrom from);
    }

    /// <inheritdoc />
    public interface IRequestMapper : IMapper<HttpRequest, HttpRequestMessage>
    {
    }

    /// <inheritdoc />
    public class RequestMapper : IRequestMapper
    {
        /// <summary>
        /// The default instance of <see cref="RequestMapper"/>.
        /// </summary>
        public static readonly RequestMapper Default = new RequestMapper();

        /// <summary>
        /// Headers that are excluded from the mapping.
        /// </summary>
        public readonly string[] UnsupportedHeaders = {"host"};

        /// <inheritdoc />
        public Func<HttpRequestMessage, HttpRequestMessage, bool> EqualityComparerDelegate { get; } = (a, b) => HttpRequestMessageEqualityComparer.Default.Equals(a, b);

        /// <inheritdoc />
        public async Task<Response<HttpRequestMessage>> MapAsync(HttpRequest request)
        {
            try
            {
                var requestMessage = new HttpRequestMessage
                {
                    Content = await MapContentAsync(request),
                    Method = MapMethod(request),
                    RequestUri = MapUri(request)
                };

                MapHeaders(request, requestMessage);

                return new OkResponse<HttpRequestMessage>(requestMessage);
            }
            catch (Exception ex)
            {
                return new ErrorResponse<HttpRequestMessage>(new RequestMappingError(ex));
            }
        }

        private async Task<HttpContent> MapContentAsync([NotNull] HttpRequest request)
        {
            if (request.Body == null || (request.Body.CanSeek && request.Body.Length <= 0))
            {
                return null;
            }

            if (request.Body != null)
            {
                request.EnableBuffering();
            }
   
            
            // Never change this to StreamContent again, I forgot it doesnt work in #464.
            var content = new ByteArrayContent(await ToByteArray(request.Body));
            content.Headers.TryAddWithoutValidation("Content-Type", new[] {request.ContentType});

            // if (!string.IsNullOrEmpty(request.ContentType))
            //     content.Headers.TryAddWithoutValidation("Content-Type", new[] { request.ContentType });

            AddHeaderIfExistsOnRequest("Content-Language", content, request);
            AddHeaderIfExistsOnRequest("Content-Location", content, request);
            AddHeaderIfExistsOnRequest("Content-Range", content, request);
            AddHeaderIfExistsOnRequest("Content-MD5", content, request);
            AddHeaderIfExistsOnRequest("Content-Disposition", content, request);
            AddHeaderIfExistsOnRequest("Content-Encoding", content, request);

            return content;
        }

        private static void AddHeaderIfExistsOnRequest([NotNull] string key, HttpContent content, [NotNull] HttpRequest request)
        {
            if (request.Headers != null && request.Headers.ContainsKey(key))
                content.Headers.TryAddWithoutValidation(key, request.Headers[key].ToList());
        }

        [NotNull]
        private static System.Net.Http.HttpMethod MapMethod([NotNull] HttpRequest request)
        {
            return new System.Net.Http.HttpMethod(request.Method ?? "GET");
        }

        [NotNull]
        private static Uri MapUri(HttpRequest request)
        {
            return new Uri(request.GetEncodedUrl());
        }

        private void MapHeaders([NotNull] HttpRequest request, HttpRequestMessage requestMessage)
        {
            foreach (var header in request.Headers)
            {
                if (IsSupportedHeader(header))
                {
                    requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }
        }

        private bool IsSupportedHeader(KeyValuePair<string, StringValues> header)
        {
            return !UnsupportedHeaders.Contains(header.Key.ToLower());
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "All")]
        private async Task<byte[]> ToByteArray(Stream stream)
        {
            using var reader = new StreamReader(stream, Encoding.UTF8, true, 1024, true);
            var buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer);
            stream.Position = 0;
            return buffer;
        }
    }
}
