using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Infrastructure;
using NinjaTools.FluentMockServer.API.Models;

namespace NinjaTools.FluentMockServer.API.Services
{
    internal class SetupService : ISetupService
    {
        private readonly ISetupRepository _repository;

        public SetupService(ISetupRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public Task<Setup> Add(Setup setup) => _repository.Add(setup);

        /// <inheritdoc />
        public bool TryGetMatchingSetup(HttpContext context, [CanBeNull] out Setup setup)
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
        public IAsyncEnumerable<Setup> GetAll() => _repository.GetAllAsync();
    }
}
