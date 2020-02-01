using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace NinjaTools.FluentMockServer.Utils
{
    public interface IMockServerLogger
    {
        void WriteLine([JetBrains.Annotations.NotNull] string message, params object[] args);
        void Error([JetBrains.Annotations.NotNull] string message, params object[] args);
    }

    internal class MockServerTestLogger : IMockServerLogger
    {
        /// <summary>
        /// Gets the default <see cref="MockServerTestLogger"/>.
        /// </summary>
        public static IMockServerLogger Instance => Factory.Value;

        private MockServerTestLogger(){}
        private static readonly Lazy<IMockServerLogger> Factory;
        static MockServerTestLogger()
        {
            Factory = new Lazy<IMockServerLogger>(() => new MockServerTestLogger());
        }

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
