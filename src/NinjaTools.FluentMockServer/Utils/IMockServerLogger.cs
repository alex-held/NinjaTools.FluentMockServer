using System;
using JetBrains.Annotations;

namespace NinjaTools.FluentMockServer.Utils
{
    public interface IMockServerLogger
    {
        void WriteLine([NotNull] string message, params object[] args);
        void Error([NotNull] string message, params object[] args);
    }

    public class NullMockServerLogger : IMockServerLogger
    {
        private static readonly Lazy<IMockServerLogger> _factory = new Lazy<IMockServerLogger>(() => new  NullMockServerLogger());

        public static IMockServerLogger Instance => _factory.Value;

        /// <inheritdoc />
        public void WriteLine(string message, params object[] args)
        {
            // Null implementation
        }

        /// <inheritdoc />
        public void Error(string message, params object[] args)
        {
            // Null implementation
        }
    }

    public class MockServerTestLogger : IMockServerLogger
    {
        private static readonly Lazy<IMockServerLogger> _factory = new Lazy<IMockServerLogger>(() => new MockServerTestLogger());

        public static IMockServerLogger Instance => _factory.Value;

        /// <inheritdoc />
        public void WriteLine(string message, [CanBeNull] params object[] args)
        {
            Console.WriteLine(message, args);
        }

        /// <inheritdoc />
        public void Error(string message, [CanBeNull] params object[] args)
        {
            Console.Error.WriteLine(message, args);
        }
    }
}
