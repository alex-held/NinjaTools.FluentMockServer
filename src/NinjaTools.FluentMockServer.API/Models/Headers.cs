using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.API.Proxy.Visitors;

namespace NinjaTools.FluentMockServer.API.Models
{

    [JsonDictionary]
    public class Headers : IVisitable, IDictionary<string, string[]>
    {
        public Headers()
        {
            Header = new Dictionary<string, string[]>();
        }

        public IDictionary<string, string[]> Header { get; }

        /// <inheritdoc cref="Headers()" />
        public Headers(IDictionary<string, string[]> dictionary) : this()
        {
            dictionary ??= new Dictionary<string, string[]>();
            foreach (var (k, v) in dictionary)
                Header.Add(k, v);
        }

        /// <inheritdoc cref="Headers()" />
        public Headers(params (string, string[])[] kvps) : this()
        {
            kvps ??= new (string, string[])[0];
            foreach (var (k, v) in kvps)
                Header.Add(k, v);
        }



        /// <inheritdoc />
        public void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<Headers> typed)
            {
                typed.Visit(this);
            }
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<string, string[]>> GetEnumerator()
        {
            return Header.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) Header).GetEnumerator();
        }

        /// <inheritdoc />
        public void Add(KeyValuePair<string, string[]> item)
        {
            Header.Add(item);
        }

        /// <inheritdoc />
        public void Clear()
        {
            Header.Clear();
        }

        /// <inheritdoc />
        public bool Contains(KeyValuePair<string, string[]> item)
        {
            return Header.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<string, string[]>[] array, int arrayIndex)
        {
            Header.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<string, string[]> item)
        {
            return Header.Remove(item);
        }

        /// <inheritdoc />
        public int Count => Header.Count;

        /// <inheritdoc />
        public bool IsReadOnly => Header.IsReadOnly;

        /// <inheritdoc />
        public void Add(string key, string[] value)
        {
            Header.Add(key, value);
        }

        /// <inheritdoc />
        public bool ContainsKey(string key)
        {
            return Header.ContainsKey(key);
        }

        /// <inheritdoc />
        public bool Remove(string key)
        {
            return Header.Remove(key);
        }

        /// <inheritdoc />
        public bool TryGetValue(string key, out string[] value)
        {
            return Header.TryGetValue(key, out value);
        }

        /// <inheritdoc />
        public string[] this[string key]
        {
            get => Header[key];
            set => Header[key] = value;
        }

        /// <inheritdoc />
        public ICollection<string> Keys => Header.Keys;

        /// <inheritdoc />
        public ICollection<string[]> Values => Header.Values;
    }
}
