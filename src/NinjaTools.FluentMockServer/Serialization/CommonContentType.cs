using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using JetBrains.Annotations;

namespace NinjaTools.FluentMockServer.Serialization
{
    /// <summary>
    ///     Contains information about various content types.
    /// </summary>
    public class CommonContentType : ContentType
    {
        public static readonly CommonContentType Json = new CommonContentType("application/json", 0);

        public static readonly CommonContentType Xml = new CommonContentType("text/xml", 1);

        public static readonly CommonContentType Soap12 = new CommonContentType("application/soap+xml", 2);

        public static readonly CommonContentType Plain = new CommonContentType("text/plain", 3);

        public static readonly CommonContentType Html = new CommonContentType("text/html", 4);

        private CommonContentType(string contentType, int id)
        {
            Name = contentType;
            Id = id;
        }

        public int Id { get; }

        /// <summary>
        ///     The MIME content type
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        [Pure]
        public static implicit operator string([NotNull] CommonContentType contentType)
        {
            return contentType.Name;
        }

        
        // /// <summary>
        // ///     Add charset information to the content type.
        // /// </summary>
        // /// <remarks>
        // ///     text/xml; --> text/xml; charset="utf-8"
        // /// </remarks>
        // /// <param name="charset">The charset of the encoding used.</param>
        // [NotNull]
        // public CommonContentType WithCharset(string charset)
        // {
        //     CharSet = charset;
        //     return this;
        // }

        [Pure]
        [NotNull]
        public static IReadOnlyList<CommonContentType> ToList()
        {
            var type = typeof(CommonContentType);
            return type
                    .GetFields(BindingFlags.Public | BindingFlags.Static
                                                   | BindingFlags.FlattenHierarchy)
                    .Where(p => type.IsAssignableFrom(p.FieldType))
                    .Select(pi => (CommonContentType) pi.GetValue(null))
                    .ToList();
        }


        /// <summary>
        ///     Tries to parse content types from user input
        /// </summary>
        /// <param name="input"></param>
        /// <param name="contentType"></param>
        /// <returns>Whether we could parse the input content type corretly.</returns>
        [Pure]
        public static bool TryParseContentType([NotNull] string input, [CanBeNull] out CommonContentType contentType)
        {
            contentType = null;
            var list = ToList();

            if (!list.Any(ct => ct.Name == input)) return false;
            contentType = list.First(ct => ct.Name == input);
            return true;
        }
    }
}
