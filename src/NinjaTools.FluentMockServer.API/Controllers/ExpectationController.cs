using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.API.Services;
using NinjaTools.FluentMockServer.Models;

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
            var counter = 1;
            await foreach (var expectation in _expectationService.GetAllAsync())
            {
                var c = counter;
                _logger.LogDebug($"[GetAsync] has found the: {c.ToString()} {nameof(Expectation)}.");
                counter++;
                yield return expectation;
            } 
                  
        }
        
        [HttpGet("prune")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IStatusCodeActionResult> Prune()
        {
            var count = await _expectationService.PruneAsync();
            return Ok(count);
        }
        
        [HttpGet("seed/{count}")]
        [Produces("application/json", Type = typeof(IAsyncEnumerable<Expectation>))]
        [ProducesResponseType(typeof(IAsyncEnumerable<Expectation>), StatusCodes.Status200OK)]
        public async IAsyncEnumerable<Expectation> SeedAsync(int count)
        {
            await foreach (var expectation in _expectationService.SeedAsync(count))
            {
                yield return expectation;
            }
        }
        
        [HttpPut("create")]
        [Consumes("application/json")]
        [Produces("application/json", Type = typeof(Expectation))]
        [ProducesResponseType(typeof(Expectation), StatusCodes.Status200OK)]
        public async Task<ActionResult<Expectation>> Create(Expectation expectation)
        { 
            var id = await _expectationService.AddAsync(expectation);
            
            if (id.HasValue)
            {
                expectation.Id = id.Value;
                return new JsonResult(expectation)
                {
                    StatusCode = 200
                };
            }

            return Problem($"Unable to save the{nameof(expectation)} on the database.",
                
                JsonConvert.SerializeObject(expectation), 500, "ServerError");

        }
    }
}
