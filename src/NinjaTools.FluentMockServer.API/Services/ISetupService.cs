using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Models;

namespace NinjaTools.FluentMockServer.API.Services
{
    public interface ISetupService
    {
        Task<Setup> Add([NotNull] Setup setup);
        bool TryGetMatchingSetup([NotNull] HttpContext context, out Setup setup);
        IAsyncEnumerable<Setup> GetAll();
    }
}
