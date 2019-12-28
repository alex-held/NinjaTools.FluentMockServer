using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NinjaTools.FluentMockServer.API.Administration
{
    public interface IAdministrationService
    {
        Task<HttpContext> HandleAsync(HttpContext context, PathString path);
    }
}
