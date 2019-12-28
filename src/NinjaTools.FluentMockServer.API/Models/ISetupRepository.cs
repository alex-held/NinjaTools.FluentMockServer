using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace NinjaTools.FluentMockServer.API.Models
{
    public interface ISetupRepository
    {
        IEnumerable<Setup> GetAll();

        void Add(Setup setup);

        public Setup TryGetMatchingSetup(HttpContext context);
    }
}
