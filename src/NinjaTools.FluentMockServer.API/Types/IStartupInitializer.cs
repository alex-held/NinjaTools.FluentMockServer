using JetBrains.Annotations;

namespace NinjaTools.FluentMockServer.API.Types
{
    /// <summary>
    /// Initializes registered <see cref="IInitializer"/> at startup.
    /// </summary>
    public interface IStartupInitializer : IInitializer
    {
        /// <summary>
        /// Registers a <see cref="IInitializer"/>
        /// </summary>
        /// <param name="initializer">The <see cref="IInitializer"/> to be initialized at startup.</param>
        void AddInitializer<TInitializer>([NotNull] TInitializer initializer) where TInitializer :class, IInitializer;
    }
}
