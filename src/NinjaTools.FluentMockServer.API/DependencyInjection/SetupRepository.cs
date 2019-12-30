using System.Collections.Generic;
using System.Linq;
using System.Net;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Models;
using HttpResponse = NinjaTools.FluentMockServer.API.Models.HttpResponse;

namespace NinjaTools.FluentMockServer.API.DependencyInjection
{
    internal sealed class SetupRepository : ISetupRepository
    {
        private readonly List<Setup> _setups;

        public SetupRepository()
        {
#if DEBUG
            _setups = new List<Setup>()
            {
                new Setup
                {
                    Matcher = new RequestMatcher
                    {
                        Path = "/some/test"
                    },
                    Action = new ResponseAction
                    {
                      Response = new HttpResponse()
                      {
                          StatusCode = (int) HttpStatusCode.Accepted,
                          Body = "hello world!"
                      }
                    }
                }
            };
#else
            _setups = new List<Setup>();
#endif
        }

        public IEnumerable<Setup> GetAll() => _setups;

        public void Add(Setup setup) => _setups.Add(setup);

        /// <inheritdoc />
        [CanBeNull]
        public Setup TryGetMatchingSetup([System.Diagnostics.CodeAnalysis.NotNull] HttpContext context)
        {
            var request = context.Request;
            var path = request.Path;

            return GetAll().FirstOrDefault(s => s.Matcher.Path.Contains(path)) is {} setup
                ? setup
                : null;
        }
    }
}
