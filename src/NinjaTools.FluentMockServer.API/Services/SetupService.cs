using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Infrastructure;
using NinjaTools.FluentMockServer.API.Models;

namespace NinjaTools.FluentMockServer.API.Services
{
    public class SetupService : ISetupService
    {
        private readonly ISetupRepository _repository;

        public SetupService(ISetupRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public void Add(Setup setup) => _repository.Add(setup);

        /// <inheritdoc />
        public bool TryGetMatchingSetup(HttpContext context, out Setup setup)
        {
            if (_repository.TryGetMatchingSetup(context) is {} matchingSetup)
            {
                setup = matchingSetup;
                return true;
            }

            setup = null;
            return false;
        }

        /// <inheritdoc />
        public IEnumerable<Setup> GetAll() => _repository.GetAll();
    }
}
