using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Models;
using NinjaTools.FluentMockServer.API.Proxy.Visitors;
using NinjaTools.FluentMockServer.API.Services;

namespace NinjaTools.FluentMockServer.API.Infrastructure
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + "}")]
    internal sealed class SetupRepository : ISetupRepository
    {
        public string DebuggerDisplay() => $"Count={Count};";

        public int Count => _setups.Count;
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
            var matches = GetMatches(context);
            var topMatch = matches.FirstOrDefault();
            var topSetup = topMatch?.Setup;
            return topSetup;
        }

        /// <inheritdoc />
        public IOrderedEnumerable<Match> GetMatches(HttpContext context)
        {
            var visitor = new RequestMatcherEvaluationVisitor(context);

            var matches = _setups
                .Select(setup => new {setup, score = visitor.Visit(setup.Matcher)})
                .Where(t => t.score > 0)
                .Select(t => new Match(t.setup, t.score))
                .OrderByDescending(m => m.Score)
                .ThenByDescending(m => m.Rank);

            return matches;
        }
    }

}
