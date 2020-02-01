using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Xunit.Attributes;
using NinjaTools.FluentMockServer.Xunit.Attributes.NinjaTools.FluentMockServer.Xunit.Attributes;
using Xunit;

namespace NinjaTools.FluentMockServer.Xunit
{
      [GlobalSetUp]
    public static class GlobalSetup
    {
        [UsedImplicitly]
        public static void Setup()
        {
            Called = true;
            ContextRegistry.Instance.RegisterCollections(ResolveCollections());
            ContextRegistry.Instance.RegisterIsolated(ResolveIsolated());
        }

        [NotNull]
        private static ConcurrentDictionary<MethodInfo, string> ResolveCollections()
        {
            var dict = new ConcurrentDictionary<MethodInfo, string>();

            var types = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => a.FullName.StartsWith("Ninja"))
                .SelectMany(a => a.GetExportedTypes())
                .ToList();

            var isolated = types
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                .Where(t => t.IsDefined(typeof(MockServerCollectionAttribute)))
                .ToList();

            foreach (var method in isolated)
            {
                var attribute = method.GetCustomAttribute<MockServerCollectionAttribute>();
                dict.TryAdd(method, attribute.Id);
            }

            return dict;
        }


        [NotNull]
        private static ConcurrentBag<MethodInfo> ResolveIsolated()
        {
            var dict = new ConcurrentBag<MethodInfo>();

            var types = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => a.FullName.StartsWith("Ninja"))
                .SelectMany(a => a.GetExportedTypes())
                .ToList();

            var isolated = types
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                .Where(t => t.IsDefined(typeof(IsolatedMockServerSetupAttribute)))
                .ToList();

            foreach (var method in isolated)
            {
                dict.Add(method);
            }

            return dict;
        }

        /// <summary>
        /// Whether the <see cref="GlobalSetup"/> has already been executed.
        /// </summary>
        public static bool Called;
    }
}
