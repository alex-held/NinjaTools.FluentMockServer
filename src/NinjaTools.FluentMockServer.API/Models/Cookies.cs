using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.API.Proxy.Visitors;
using NinjaTools.FluentMockServer.API.Types;

namespace NinjaTools.FluentMockServer.API.Models
{

    [JsonDictionary]
    public class Cookies : IVisitable,  IDictionary<string, string>, IRequestCookieCollection, IScoreable, IContentValidatable, IEquatable<Cookies>
    {
        /// <inheritdoc />
        [JsonIgnore]
        public int Score => Cookie.Count;

        /// <inheritdoc />
        public bool HasContent() => Cookie.Any();

        public IDictionary<string, string> Cookie { get; }= new Dictionary<string, string>();

        public Cookies(params (string key, string value)[] args)
        {
            foreach (var (k, v) in (args ??= new (string key, string value)[0]))
                Cookie.Add(k, v);
        }

        /// <inheritdoc />
        public void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<Cookies> typed)
            {
                typed.Visit(this);
            }
        }


        #region Equals

        /// <inheritdoc />
        public bool Equals(Cookies? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Cookie.Equals(other.Cookie);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Cookies) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Cookie.GetHashCode();
        }

        public static bool operator ==(Cookies? left, Cookies? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Cookies? left, Cookies? right)
        {
            return !Equals(left, right);
        }

        #endregion

        #region Implementation of IDictionary

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

        /// <inheritdoc cref="Cookie" />
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

        /// <inheritdoc cref="Cookie" />
        public ICollection<string> Keys => Cookie.Keys;

        /// <inheritdoc />
        public ICollection<string> Values => Cookie.Values;

        #endregion


    }
}
