using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.FluentAPI.Builders;
using NinjaTools.FluentMockServer.Models;

namespace NinjaTools.FluentMockServer.Serialization
{
    public class ExpectationConverter : JsonConverter<Expectation>
    {
        private static readonly object Sync = new object();

        private bool _isWriting;

        private bool IsWriting
        {
            get
            {
                lock (Sync)
                {
                    return _isWriting;
                }
            }

            set
            {
                lock (Sync)
                {
                    _isWriting = value;
                }
            }
        }


        /// <inheritdoc />
        public override void WriteJson([NotNull] JsonWriter writer, Expectation value, JsonSerializer serializer)
        {
            try
            {
                if (IsWriting) return;
                IsWriting = true;
                var json = Serializer.Serialize(value);
                writer.WriteRawValue(json);
            }
            finally
            {
                IsWriting = false;
            }
        }

        /// <inheritdoc />
        public override Expectation ReadJson(JsonReader reader, [NotNull] Type objectType, Expectation existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var token = JToken.ReadFrom(reader);

            var expectation = FluentExpectationBuilder.Create();
            
            foreach (var propertyInfo in objectType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite && !p.IsSpecialName))
            {
                var propertyName = propertyInfo.Name;
                var firstLetter = propertyName.First().ToString().ToLower();
                var camelCasePropertyName = firstLetter + propertyName.Substring(1);

                if (!(token.SelectToken(camelCasePropertyName, false) is {} memberJson)) continue;
                try
                {
                    var value = memberJson.ToObject(propertyInfo.PropertyType);
                    propertyInfo.SetValue(expectation, value, BindingFlags.Instance | BindingFlags.NonPublic, null, null, CultureInfo.DefaultThreadCurrentCulture);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
//            if (token.SelectToken("httpRequest", false) is {} httpRequest)
//                try
//                {
//                    expectation.HttpRequest = httpRequest.ToObject<HttpRequest>();
//                }
//                catch (Exception e)
//                {
//                    Console.WriteLine(e);
//                }

            return expectation;
        }
    }
}
