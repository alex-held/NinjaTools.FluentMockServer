using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("NinjaTools.FluentMockServer.API.Tests")]
namespace NinjaTools.FluentMockServer.API.Types
{
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
