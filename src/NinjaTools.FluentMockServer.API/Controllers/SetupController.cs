using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using NinjaTools.FluentMockServer.API.Models;
using NinjaTools.FluentMockServer.API.Services;

namespace NinjaTools.FluentMockServer.API.Controllers
{
    /// <summary>
    /// The API to administer <see cref="Setup"/>.
    /// </summary>
    [ApiController]
    [Route("setup")]
    public class SetupController : ControllerBase
    {
        private readonly ISetupService _setupService;

        /// <inheritdoc />
        public SetupController(ISetupService setupService)
        {
            _setupService = setupService;
        }

        /// <summary>
        /// Gets all active <see cref="Setup"/> on the Mock-Server.
        /// </summary>
        /// <returns>A list of <see cref="Setup"/>.</returns>
        [HttpGet("list")]
        public IEnumerable<Setup> List()
        {
            var setups = _setupService.GetAll();
            return setups;
        }

        /// <summary>
        /// Configures a new <see cref="Setup"/> on the Mock-Server.
        /// </summary>
        /// <param name="setup"></param>
        /// <returns>The configured <see cref="Setup"/></returns>
        [HttpPost("create")]
        public Task<Setup> Create([NotNull] Setup setup)
        {
             _setupService.Add(setup);
             return Task.FromResult(setup);
        }
    }
}
