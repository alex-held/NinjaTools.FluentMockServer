using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Models;

namespace NinjaTools.FluentMockServer.API.Services
{
    public interface ISetupService
    {
        void Add([NotNull] Setup setup);
        bool TryGetMatchingSetup([NotNull] HttpContext context, out Setup setup);
        IEnumerable<Setup> GetAll();
    }
}
