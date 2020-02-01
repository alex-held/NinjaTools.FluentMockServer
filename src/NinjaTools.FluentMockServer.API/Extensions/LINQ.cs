using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NinjaTools.FluentMockServer.API.Models;

namespace NinjaTools.FluentMockServer.API.Extensions
{
    public static class LINQ
    {


        public static Dictionary<string, string[]> ToDictionary<T>(this T headers) where T : IHeaderDictionary
        {
            return headers.ToDictionary(
                k => k.Key,
                v => v.Value.ToArray());
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

        public static Headers ToHeaderCollection<T>(this T headers) where T : IHeaderDictionary
        {
            return new Headers(headers.ToDictionary(
                k => k.Key,
                v => v.Value.ToArray()));
        }

        public static HeaderDictionary ToHeaderCollection(this IDictionary<string, string[]> headers)
        {
            return new HeaderDictionary(headers.ToDictionary(k => k.Key,
                v => new StringValues(v.Value)));
        }

        public static HeaderDictionary ToHeaderCollection(this IDictionary<string, StringValues> headers)
        {
            return new HeaderDictionary(headers.ToDictionary(k => k.Key, v => v.Value));
        }


        public static Dictionary<string, string[]> ToDictionary(this IDictionary<string, StringValues> headers)
        {
            return headers.ToDictionary(k => k.Key, v => v.Value.ToArray());
        }
    }
}
