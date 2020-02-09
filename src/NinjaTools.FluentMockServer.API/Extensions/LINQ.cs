using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Models;

namespace NinjaTools.FluentMockServer.API.Extensions
{
    public static class LINQ
    {
        /// <summary>
        /// Returns the input <see cref="IEnumerable{T}"/> without elements equal to <c>null</c> or <![CDATA[default{T}]]>.
        /// </summary>
        /// <exception cref="ArgumentNullException">The parameter <param name="input"/> was null.</exception>
        public static IEnumerable<T> SkipNullOrDefault<T>([CanBeNull] this IEnumerable<T>? input)
        {
            return (input ?? throw new ArgumentNullException(nameof(input)))
                .TakeWhile(value => !(value is null) && !value.Equals(default(T)));
        }

        [NotNull]
        public static IReadOnlyList<T> EnsureNotNullNotEmpty<T>([CanBeNull] this IEnumerable<T>? input)
        {
            var collection = (input ?? throw new InvalidOperationException(nameof(input))).ToList();

            if (!collection.Any())
                throw new InvalidOperationException("EnsureNotNullNotEmpty<T>; EMPTY SEQUENCE");

            if (collection.Any(item => item is null))
                throw new InvalidOperationException("EnsureNotNullNotEmpty<T>; NULL ITEM IN SEQUENCE");

            return collection;
        }

        public static Headers ToHeaders<T>(this T headers) where T : IHeaderDictionary
        {
            return new Headers(headers.ToDictionary(
                k => k.Key,
                v => v.Value.ToArray()));
        }

        public static Headers? ToHeadersOrDefault<T>(this T headers) where T : IHeaderDictionary
        {
            return headers?.Any() ?? false
                   ? headers.ToHeaders()
                   : null;
        }

        public static Cookies ToCookies<T>([CanBeNull] this T cookies) where T : IRequestCookieCollection
        {
            return new Cookies(cookies.ToDictionary(k => k.Key, v => v.Value));
        }

        public static Cookies? ToCookiesOrDefault<T>([CanBeNull] this T cookies) where T : IRequestCookieCollection
        {
            return new Cookies(cookies?.ToDictionary(k => k.Key, v => v.Value));
        }
    }
}
