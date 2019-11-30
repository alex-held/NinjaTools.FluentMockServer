namespace NinjaTools.FluentMockServer.Extensions
{
    public static class StringExtensions
    {
        public static bool StartsWithEither(this string @string, params char[] chars)
        {
            foreach (var @char in chars)
                if (@string.StartsWith(@char.ToString()))
                    return true;

            return false;
        }

        public static bool EndsWithEither(this string @string, params char[] chars)
        {
            foreach (var @char in chars)
                if (@string.EndsWith(@char.ToString()))
                    return true;

            return false;
        }

        public static bool IsContainerByEither(this string @string, params char[] chars)
        {
            return @string.StartsWithEither(chars) && @string.EndsWithEither(chars);
        }
    }
}
