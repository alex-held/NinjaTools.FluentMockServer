using System.Collections.Generic;
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
    }
}
