using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Xunit;

namespace NinjaTools.FluentMockServer.Xunit
{
    public class ContextRegistry
    {
        private static readonly Lazy<ContextRegistry> _factory = new Lazy<ContextRegistry>(() => new ContextRegistry());
        public static ContextRegistry Instance => _factory.Value;
        private ContextRegistry(){}

        public ConcurrentDictionary<MethodInfo, string> Collections { get; } = new ConcurrentDictionary<MethodInfo, string>();
        public ConcurrentBag<MethodInfo> Isolated { get; } = new ConcurrentBag<MethodInfo>();

        internal bool TryGetIdentifier(MethodInfo mi, out string id)
        {
            if (!Isolated.Contains(mi))
                return Collections.TryGetValue(mi, out id);
            id =  XunitContext.Context.UniqueTestName;
            return true;
        }

        internal  void RegisterIsolated([NotNull] ConcurrentBag<MethodInfo> isolated )
        {
            foreach (var methodInfo in isolated)
            {
                Isolated.Add(methodInfo);
            }
        }

        internal  void RegisterCollections([NotNull] ConcurrentDictionary<MethodInfo,string> collections)
        {
            foreach (var kvp in collections)
            {
                Collections.TryAdd(kvp.Key, kvp.Value);
            }
        }
    }
}
