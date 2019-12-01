using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using NinjaTools.FluentMockServer.API.Services;
using NinjaTools.FluentMockServer.Domain.Models;

namespace NinjaTools.FluentMockServer.API.Controllers
{
    [ApiController]
    [Route("expectation")]
    public class ExpectationController : ControllerBase
    {
        private readonly ILogger<ExpectationController> _logger;
        private readonly ExpectationService _expectationService;

        public ExpectationController(ILogger<ExpectationController> logger, ExpectationService expectationService)
        {
            _logger = logger;
            _expectationService = expectationService;
        }
        
        [HttpGet("list")]
        [ProducesResponseType(typeof(List<Expectation>), StatusCodes.Status200OK)]

        public async IAsyncEnumerable<Expectation> GetAsync()
        {
              await foreach (var expectation in _expectationService.GetAllAsync()) 
                  yield  return  expectation;
        }
        
        [HttpGet("prune")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IStatusCodeActionResult> Prune()
        {
            var count = await _expectationService.PruneAsync();
            return Ok(count);
        }
        
        [HttpGet("seed/{count}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IStatusCodeActionResult Seed(int count)
        {
            _expectationService.SeedAsync(count);
            return Ok();
        }
        
        [HttpGet("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IStatusCodeActionResult> Create(Expectation expectation)
        { 
            await _expectationService.AddAsync(expectation);
            return Ok();
        }
    }
}
