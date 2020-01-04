using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using NinjaTools.FluentMockServer.API.Models;
using NinjaTools.FluentMockServer.API.Services;

namespace NinjaTools.FluentMockServer.API.Controllers
{
    [ApiController]
    [Route("setup")]
    public class SetupController : ControllerBase
    {
        private readonly ISetupService _setupService;

        public SetupController(ISetupService setupService)
        {
            _setupService = setupService;
        }

        [HttpGet("list")]
        public IEnumerable<Setup> List()
        {
            var setups = _setupService.GetAll();
            return setups;
        }

        [HttpPost("create")]
        public Task<Setup> Create([NotNull] Setup setup)
        {
             _setupService.Add(setup);
             return Task.FromResult(setup);
        }
    }
}
