using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.Domain.Builders;
using Formatting = Newtonsoft.Json.Formatting;

namespace NinjaTools.FluentMockServer.Domain.Utils
{
    /// <summary>
    ///     Use this tool to reliable use the same encoding and decoding technique.
    /// </summary>
    internal static class EncodingUtils<T>
    {
        /// <summary>
        ///     Use this tool to reliable use the same encoding and decoding technique.
        /// </summary>
        /// <param name="body">An instance of an object to serialize.</param>
        /// <param name="contentType">The content-type will determinate how to serialize the content.</param>
        /// <param name="encoding">Here you may like to specify a different encoding to use.</param>
        /// <returns>A byte array containing payload.</returns>
        /// <exception cref="ArgumentNullException">The
        ///     <param name="body" />
        ///     might have been null.
        /// </exception>
        /// <exception cref="ArgumentNullException">The
        ///     <param name="contentType" />
        ///     might have been null.
        /// </exception>
        /// <exception cref="NotSupportedException">The
        ///     <param name="contentType" />
        ///     is currently not supported.
        /// </exception>
        [System.Diagnostics.Contracts.Pure]
        public static byte[] Encode([NotNull] T body, [NotNull] string contentType, [NotNull] Encoding encoding)
        {
            if (body is null)
                throw new ArgumentNullException(nameof(body));
            if (encoding is null)
                throw new ArgumentNullException(nameof(encoding));

            try
            {
                if (!CommonContentType.TryParseContentType(contentType, out var commonContentType))
                    throw new NotSupportedException($"The content type: {contentType} is currently not supported.");

                if (commonContentType.Id == CommonContentType.Json.Id) return EncodeJson(body, encoding);
                if (commonContentType.Id == CommonContentType.Xml.Id) return EncodeXml(body, encoding);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }

            return new byte[0];
        }


        [System.Diagnostics.Contracts.Pure]
        private static byte[]? EncodeJson([NotNull] T body, [NotNull] Encoding encoding)
        {
            try
            {
                var json = JsonConvert.SerializeObject(body, Formatting.Indented);

                var bytes = encoding.GetBytes(json);

                return bytes;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }


        [System.Diagnostics.Contracts.Pure]
        private static byte[]? EncodeXml([NotNull] T body, [NotNull] Encoding encoding)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                using var stringWriter = new StringWriter();
                using var xmlWriter = XmlWriter.Create(stringWriter);
                xmlSerializer.Serialize(xmlWriter, body);
                var xml = stringWriter.ToString();

                var bytes = encoding.GetBytes(xml);
                return bytes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }
    }
}
