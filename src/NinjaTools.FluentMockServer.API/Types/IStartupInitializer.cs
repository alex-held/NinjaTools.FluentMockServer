using JetBrains.Annotations;

namespace NinjaTools.FluentMockServer.API.Types
{
    /// <summary>
    /// Initializes registered <see cref="T:NinjaTools.FluentMockServer.API.Types.IInitializer" /> at startup.
    /// </summary>
    internal interface IStartupInitializer : IInitializer
    {
        /// <summary>
        /// Registers a <see cref="IInitializer"/>
        /// </summary>
        /// <param name="initializer">The <see cref="IInitializer"/> to be initialized at startup.</param>
        void AddInitializer<TInitializer>([NotNull] TInitializer initializer) where TInitializer :class, IInitializer;
    }
}
