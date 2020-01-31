using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("NinjaTools.FluentMockServer.API.Tests")]
namespace NinjaTools.FluentMockServer.API.Types
{

    public interface IIdentifable<out T>
    {
        T Id { get; }
    }


    /// <summary>
    /// Initializes this instance.
    /// </summary>
    public interface IInitializer
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns></returns>
        Task InitializeAsync();
    }
}
