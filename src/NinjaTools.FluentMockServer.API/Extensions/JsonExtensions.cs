using System;
using System.ComponentModel;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace NinjaTools.FluentMockServer.API.Extensions
{
    public static class JsonExtensions
    {
        public static bool IsNullOrEmpty(this JToken token)
        {
            return (token == null) ||
                   (token.Type == JTokenType.Array && !token.HasValues) ||
                   (token.Type == JTokenType.Object && !token.HasValues) ||
                   (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
                   (token.Type == JTokenType.Null);
        }

        public static bool IsValidJson(this string text)
        {
            text = text.Trim();
            if ((text.StartsWith("{") && text.EndsWith("}")) || //For object
                (text.StartsWith("[") && text.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(text);
                    return true;
                }
                catch(Exception) {
                    return false;
                }
            }
            else
            {
                return false;
            }
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
