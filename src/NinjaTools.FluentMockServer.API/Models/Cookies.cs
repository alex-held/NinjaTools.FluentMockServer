using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.API.Proxy.Visitors;

namespace NinjaTools.FluentMockServer.API.Models
{

    [JsonDictionary]
    public class Cookies : IVisitable,  IDictionary<string, string>
    {
        public IDictionary<string, string> Cookie { get; } = new Dictionary<string, string>();

        public Cookies(params (string key, string value)[] args)
        {
            foreach (var (k, v) in (args ??= new (string key, string value)[0]))
                Cookie.Add(k, v);
        }

        public Cookies(IDictionary<string,string> dict) : this(dict?.Select(kvp => (kvp.Key, kvp.Value))?.ToArray())
        { }

        /// <inheritdoc />
        public void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<Cookies> typed)
            {
                typed.Visit(this);
            }
        }


        /// <inheritdoc />
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return Cookie.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) Cookie).GetEnumerator();
        }

        /// <inheritdoc />
        public void Add(KeyValuePair<string, string> item)
        {
            Cookie.Add(item);
        }

        /// <inheritdoc />
        public void Clear()
        {
            Cookie.Clear();
        }

        /// <inheritdoc />
        public bool Contains(KeyValuePair<string, string> item)
        {
            return Cookie.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            Cookie.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<string, string> item)
        {
            return Cookie.Remove(item);
        }

        /// <inheritdoc />
        public int Count => Cookie.Count;

        /// <inheritdoc />
        public bool IsReadOnly => Cookie.IsReadOnly;

        /// <inheritdoc />
        public void Add(string key, string value)
        {
            Cookie.Add(key, value);
        }

        /// <inheritdoc />
        public bool ContainsKey(string key)
        {
            return Cookie.ContainsKey(key);
        }

        /// <inheritdoc />
        public bool Remove(string key)
        {
            return Cookie.Remove(key);
        }

        /// <inheritdoc />
        public bool TryGetValue(string key, out string value)
        {
            return Cookie.TryGetValue(key, out value);
        }

        /// <inheritdoc />
        public string this[string key]
        {
            get => Cookie[key];
            set => Cookie[key] = value;
        }

        /// <inheritdoc />
        public ICollection<string> Keys => Cookie.Keys;

        /// <inheritdoc />
        public ICollection<string> Values => Cookie.Values;
    }

    /// <inheritdoc />
    public class CookieCollectionConverter : JsonConverter<Cookies>
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, Cookies value, JsonSerializer serializer)
        {
            var t = JToken.FromObject(value);
            if (t.Type == JTokenType.Object)
            {
                t.WriteTo(writer);
            }
        }

        /// <inheritdoc />
        public override Cookies ReadJson(JsonReader reader, Type objectType, Cookies existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jo = JToken.ReadFrom(reader);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jo.ToString());
            return new Cookies(dict);
        }
    }
}
