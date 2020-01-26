using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NinjaTools.FluentMockServer.API.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidRegex(this string pattern)
        {
            if (string.IsNullOrEmpty(pattern)) return false;

            try
            {
                Regex.Match("", pattern);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        public static string GetDescription<T>(this T e) where T : Enum, IConvertible
        {
            var type = typeof(T);
            var values = Enum.GetValues(type);

            foreach (int val in values)
            {
                if (val != e.ToInt32(CultureInfo.InvariantCulture))
                {
                    continue;
                }

                var memInfo = type.GetMember(type.GetEnumName(val));
                var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (descriptionAttributes.Length > 0)
                {
                    return ((DescriptionAttribute)descriptionAttributes[0]).Description;
                }
            }

            throw new InvalidOperationException($"Could not find any  {nameof(DescriptionAttribute)} on members of enum {type.Name}.");
        }
    }
}
