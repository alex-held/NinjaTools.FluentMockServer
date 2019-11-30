using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
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
        public IEnumerable<Expectation> Get()
        {
            return _expectationService.GetExpectations();
        }
        
        [HttpGet("prune")]
        public IActionResult Prune()
        {
            return Ok(_expectationService.Prune());
        }
        
        [HttpGet("seed/{count}")]
        public IActionResult Seed(int count)
        {
            _expectationService.Seed(count);
            return Ok();
        }
        
        [HttpGet("create")]
        public IActionResult Create(Expectation expectation)
        {
          _expectationService.Add(expectation);
           return Accepted();
        }
    }
}
