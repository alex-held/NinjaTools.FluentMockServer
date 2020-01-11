using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Models;
using NinjaTools.FluentMockServer.API.Services;

namespace NinjaTools.FluentMockServer.API.Infrastructure
{
    internal sealed class SetupRepository : ISetupRepository
    {
        private readonly List<Setup> _setups;

        private readonly ILogService _logService;

        public SetupRepository(ILogService logService)
        {
            _logService = logService;
            _setups = new List<Setup>();
        }

        public IEnumerable<Setup> GetAll() => _setups;

        public void Add(Setup setup)
        {
            _setups.Add(setup);
            _logService.Log(log => log.SetupCreated(setup));
        }

        /// <inheritdoc />
        [CanBeNull]
        public Setup? TryGetMatchingSetup([NotNull] HttpContext context)
        {
            return GetAll().FirstOrDefault(s => s.Matcher?.IsMatch(context) ?? false) is {} setup
                ? setup
                : null;
        }
    }
}
