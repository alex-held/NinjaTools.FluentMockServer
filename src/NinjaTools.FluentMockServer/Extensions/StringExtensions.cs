using System.Linq;

namespace NinjaTools.FluentMockServer.Extensions
{
    internal static class StringExtensions
    {
        public static bool StartsWithEither(this string @string, params char[] chars)
        {
            return chars.Any(@char => @string.StartsWith(@char.ToString()));
        }

        public static bool EndsWithEither(this string @string, params char[] chars)
        {
            return chars.Any(@char => @string.EndsWith(@char.ToString()));
        }

        public static bool IsContainerByEither(this string @string, params char[] chars)
        {
            return @string.StartsWithEither(chars) && @string.EndsWithEither(chars);
        }
    }
}
