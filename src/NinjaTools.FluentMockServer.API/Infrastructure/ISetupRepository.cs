using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Models;

namespace NinjaTools.FluentMockServer.API.Infrastructure
{
    public interface ISetupRepository
    {
        IEnumerable<Setup> GetAll();

        void Add(Setup setup);

        public Setup? TryGetMatchingSetup(HttpContext context);
    }
}
