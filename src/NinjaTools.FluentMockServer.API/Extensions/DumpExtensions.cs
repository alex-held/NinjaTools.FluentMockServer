using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NinjaTools.FluentMockServer.API.Configuration;
using NinjaTools.FluentMockServer.Serialization;
using JsonProperty = Newtonsoft.Json.Serialization.JsonProperty;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace NinjaTools.FluentMockServer.API.Extensions
{
    internal class HttpContentConverter : JsonConverter<HttpContent>
    {
        /// <inheritdoc />
        public override void WriteJson([NotNull] JsonWriter writer, [NotNull] HttpContent value, [NotNull] JsonSerializer serializer)
        {
            //  value.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
            //      var json = value.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.Formatting = Formatting.Indented;
            serializer.Serialize(writer, new
            {
                Content = value.ReadAsStringAsync(),
                Headers = value.Headers.ToDictionary(key => key.Key, val => val.Value)
            });
        }

        /// <inheritdoc />
        [NotNull]
        public override HttpContent ReadJson(JsonReader reader,
            Type objectType, HttpContent existingValue,
            bool hasExistingValue,
            [NotNull] JsonSerializer serializer)
        {
            var bytes = serializer.Deserialize<byte[]>(reader);
            return new ByteArrayContent(bytes);
        }
    }

    public class MemoryStreamJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(MemoryStream).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var bytes = serializer.Deserialize<byte[]>(reader);
            return bytes != null ? new MemoryStream(bytes) : new MemoryStream();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var bytes = ((MemoryStream)value).ToArray();
            serializer.Serialize(writer, bytes);
        }
    }
    
    // internal class StreamConverter : JsonConverter<Stream>
    // {
    //     /// <inheritdoc />
    //     public override void WriteJson(JsonWriter writer, [CanBeNull] object? value, JsonSerializer serializer)
    //     {
    //         if (value is Stream stream)
    //         {
    //             var memStream = new MemoryStream();
    //             var sw = new StreamWriter(memStream);
    //             stream.CopyTo(memStream);
    //             sw.WriteLine();
    //             Encoding.UTF8.GetString(sw.WriteLine(json));
    //             
    //             var bytes = new byte[stream.Length];
    //             stream.Write(bytes);
    //         
    //          
    //         }
    //
    //         serializer.NullValueHandling = NullValueHandling.Ignore;
    //         serializer.Formatting = Formatting.Indented;
    //         serializer.Serialize(writer, new
    //         {
    //             Content = stream.Write(bytes)
    //             Headers = value.Headers.ToDictionary(key => key.Key, val => val.Value)
    //         });
    //             
    //         //     
    //         //     switch (value)
    //         //     {
    //         //         case MemoryStream memoryStream:
    //         //         {
    //         //             var base64 = Encoding.UTF8.GetString(memoryStream.ToArray());
    //         //             serializer.Serialize(writer, base64);
    //         //             break;
    //         //         }
    //         //         case Stream stream:
    //         //         {
    //         //             using (stream)
    //         //             {
    //         //                 // ReSharper disable once ConvertToUsingDeclaration
    //         //                 using (var memStream = new MemoryStream())
    //         //                 {
    //         //                     stream.CopyTo(memStream);
    //         //                     var base64 = Encoding.UTF8.GetString(memStream.ToArray());
    //         //                     writer.WriteValue(base64);
    //         //                     break;
    //         //                 }
    //         //             }
    //         //         }
    //         //         case null:
    //         //             throw new ArgumentNullException(nameof(value));
    //         //         default:
    //         //             throw new NotSupportedException($"{GetType().Name} does not support serializing {value.GetType().Name}.");
    //         //     }
    //         // }
    //         // catch (Exception e)
    //         // {
    //         //     serializer.Serialize(writer, e.Message);
    //         // }
    //     }
    //
    //     /// <inheritdoc />
    //     public override void WriteJson(JsonWriter writer, Stream value, JsonSerializer serializer)
    //     {
    //         var ms = new MemoryStream();
    //         writer.WriteValue();
    //         
    //         memStream.Write(Convert.FromBase64String(value));
    //        var base64 = Convert.ToBase64String()
    //     }
    //
    //     /// <inheritdoc />
    //     [NotNull]
    //     public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    //     {
    //         try
    //         {
    //             var bytes = serializer.Deserialize<byte[]>(reader);
    //             return bytes != null ? new MemoryStream(bytes) : new MemoryStream();
    //         }
    //         catch (Exception e)
    //         {
    //             return new MemoryStream(Encoding.UTF8.GetBytes(e.Message));
    //         }
    //     }
    //
    //     /// <inheritdoc />
    //     public override Stream ReadJson(JsonReader reader, Type objectType, Stream existingValue, bool hasExistingValue, JsonSerializer serializer)
    //     {
    //         throw new NotImplementedException();
    //     }
    //
    //     /// <inheritdoc />
    //     public override bool CanConvert([NotNull] Type objectType)
    //     {
    //         return objectType.IsSubclassOf(typeof(Stream))
    //                && !typeof(ISerializable).IsAssignableFrom(objectType)
    //                && objectType != typeof(ClaimsPrincipal)
    //                && objectType != typeof(ClaimsIdentity)
    //                && (!objectType.Name.Contains("Claims") && objectType.Assembly.FullName.Contains("System.Security"));
    //     }
    // }

    public static class DumpExtensions
    {
        /// <summary>
        /// Prettify a <see cref="string"/> to  display all the data as a indented json. 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        [NotNull]
        public static string Dump([NotNull] this object instance, [CanBeNull] string? title = null)
        {
            var sb = new StringBuilder(title ?? instance.GetType().Name);
            sb.AppendLine();

            try
            {
                var jsonContent = JsonConvert.SerializeObject(instance, new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter> {new MemoryStreamJsonConverter()}, NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.Indented
                });
                sb.AppendLine(jsonContent);
                return sb.ToString();
            }
            catch (Exception e)
            {
               
                if (instance is HttpRequest request)
                {
                    string bodyContent = null;
                    if (request.Body != null)
                    {
                        using var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true);
                        bodyContent =  reader.ReadToEndAsync().Result;
                        request.Body.Position = 0;
                    }

                    var message = JsonConvert.SerializeObject(request.Body, 
                        Formatting.Indented, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            Formatting = Formatting.Indented,
                            Converters = new List<JsonConverter>() {new MemoryStreamJsonConverter()}
                        });

                    sb.AppendLine(message);
                    return sb.ToString();
                }
            }
            
            sb.AppendLine();
            return sb.ToString();

        // var json = JsonConvert.SerializeObject(instance, instance.GetType(), new JsonSerializerSettings
            // {
            //     PreserveReferencesHandling = PreserveReferencesHandling.None,
            //     ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            //     Formatting = Formatting.Indented,
            //     ContractResolver = new DefaultContractResolver
            //     {
            //         IgnoreSerializableAttribute = true,
            //         IgnoreShouldSerializeMembers = true,
            //         IgnoreSerializableInterface = true
            //     },
            //     Converters = new List<JsonConverter>
            //     {
            //         //new StreamConverter()
            //         new HttpContentConverter()
            //     }
            // });

            // if (instance is Stream s)
            // {
            //     s.Position = 0;
            //     s.Flush();
            //     var sr = new StreamReader(s);
            //     var content = sr.ReadToEnd();
            //     sb.AppendLine(content);
            // }
            //
            // if (instance is MemoryStream m)
            // {
            //     m.Position = 0;
            //     m.Flush();
            //     var bytes = m.ToArray();
            //     var sr = new StringWriter(sb);
            //     sr.WriteLine(Convert.ToBase64String(bytes));
            // }
            // else
            // {
            //     var ms = new MemoryStream();
            //     var writer = new StreamWriter(ms);
            //     writer.WriteLine();
            //     writer.Flush();
            //     ms.Position = 0;
            //
            //     var json = JsonConvert.SerializeObject(ms, Formatting.Indented, new MemoryStreamJsonConverter());
            //     sb.AppendLine(json);
            // }

            // var json = JsonConvert.SerializeObject(instance, Formatting.Indented);
          
        }

        [NotNull]
        public static string Dump([NotNull] this List<Error> errors, [CanBeNull] string? title = null)
        {
            return string.Concat(errors.Select((error, i) =>
            {
                var errorCode = (int) error.ErrorCode;
                title ??= $"of {errors.Count}";
                var errorName = Enum.GetName(typeof(MockServerErrorCode), error.ErrorCode);
                var sb = new StringBuilder($"[Error #{i.ToString()}]\t{title}");
                sb.AppendLine();
                sb.AppendLine($"Error: {errorName}");
                sb.AppendLine($"ErrorCode: {errorCode.ToString()}");
                sb.AppendLine($"Message: {error.Message}");
                sb.AppendLine();
                return sb.ToString();
            }));
        }
    }
}
