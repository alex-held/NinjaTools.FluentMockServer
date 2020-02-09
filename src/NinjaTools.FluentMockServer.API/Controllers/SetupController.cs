using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
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
    [Produces("application/json")]
    public class SetupController : ControllerBase
    {
        private readonly ISetupService _setupService;
        private readonly IMapper _mapper;

        /// <inheritdoc />
        public SetupController(ISetupService setupService, IMapper mapper)
        {
            _setupService = setupService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all active <see cref="Setup"/> on the Mock-Server.
        /// </summary>
        /// <returns>A list of <see cref="Setup"/>.</returns>
        [HttpGet("list")]
        [ProducesResponseType(typeof(IAsyncEnumerable<SetupViewModel>), (int) HttpStatusCode.OK)]
        public async IAsyncEnumerable<SetupViewModel> List()
        {
            await foreach (var setup in _setupService.GetAll())
            {
                yield return _mapper.Map<SetupViewModel>(setup);
            }
        }

        /// <summary>
        /// Configures a new <see cref="Setup"/> on the Mock-Server.
        /// </summary>
        /// <param name="setupViewModel"></param>
        /// <returns>The configured <see cref="Setup"/></returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(IAsyncEnumerable<SetupViewModel>), (int) HttpStatusCode.Created)]
        public async Task<SetupViewModel> Create([NotNull] SetupViewModel setupViewModel)
        {
            var setup = _mapper.Map<Setup>(setupViewModel);
            var result = await _setupService.Add(setup);
            return _mapper.Map<SetupViewModel>(result);
        }
    }
}
