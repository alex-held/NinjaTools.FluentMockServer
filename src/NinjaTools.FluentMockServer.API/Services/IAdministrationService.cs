using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NinjaTools.FluentMockServer.API.Services
{
    public interface IAdministrationService
    {
        Task<HttpContext> HandleAsync(HttpContext context, PathString path);
    }
}
