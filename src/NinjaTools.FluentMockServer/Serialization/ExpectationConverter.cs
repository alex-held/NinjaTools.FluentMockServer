using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Models;
using NinjaTools.FluentMockServer.Models.HttpEntities;

namespace NinjaTools.FluentMockServer.Serialization
{
    public class ExpectationConverter : JsonConverter<Expectation>
    {
       private static readonly object sync = new object();
     
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
       
        private bool _isWriting;
        

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, Expectation value, JsonSerializer serializer)
        {

            try
            {
                if (IsWriting)
                {
                    return;
                }
                
                IsWriting = true;
//           
                
                var json = Serializer.Serialize(value);
                writer.WriteRawValue(json);
                Console.WriteLine(json);
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

            if (token.SelectToken("httpRequest", false) is {} httpRequest)
            {
                try
                {
                    expectation.HttpRequest = httpRequest.ToObject<HttpRequest>();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return expectation;
        }
    }
}
