using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Domain.Models;

namespace NinjaTools.FluentMockServer.Domain.Serialization
{
    public class ExpectationConverter : JsonConverter<Expectation>
    {
        private static readonly object sync = new object();

        private bool _isWriting;

        private bool IsWriting
        {
            get
            {
                lock (sync)
                {
                    return _isWriting;
                }
            }

            set
            {
                lock (sync)
                {
                    _isWriting = value;
                }
            }
        }


        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, Expectation value, JsonSerializer serializer)
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
        public override Expectation ReadJson(JsonReader reader, Type objectType, Expectation existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var token = JToken.ReadFrom(reader);

            var expectation = new Expectation();
            
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
                    propertyInfo.SetValue(expectation, value);
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
