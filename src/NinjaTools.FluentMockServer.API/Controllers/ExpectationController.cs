using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NinjaTools.FluentMockServer.Builders.Expectation;
using NinjaTools.FluentMockServer.Client.Models;

namespace NinjaTools.FluentMockServer.API.Controllers
{


    public class ExpectationService
    {
        private readonly List<Expectation> _expectations;
        
        public ExpectationService()
        {
            _expectations= new List<Expectation>();
            Seed(5);
        }


        public IEnumerable<Expectation> GetExpectations(Func<Expectation, bool> predicate = null)
        {
            return _expectations.Where(predicate ?? (e => true)).ToArray();
        }

        public int Prune()
        {
            var count = _expectations.Count;
            _expectations.Clear();
            return count;
        }

        public void Add(Expectation expectation) => _expectations.Add(expectation);
        
        public void Seed(int count)
        {
            if (count >= 1)
            {
                var builder = new FluentExpectationBuilder();
                var result = builder.RespondTimes(3, 200).Setup().Expectations.First();
                _expectations.Add(result);
            }
            if (count >= 1)
            {
                var builder = new FluentExpectationBuilder();
                var result = builder
                    .OnHandling(HttpMethod.Post)
                    .RespondTimes(3, HttpStatusCode.Conflict)
                    .Setup()
                    .Expectations.First();
                _expectations.Add(result);
            }
            if (count >= 2)
            {
                var builder = new FluentExpectationBuilder();
                var result = builder
                    .OnHandling(HttpMethod.Post)
                    .RespondOnce(201)
                    .Setup()
                    .Expectations.First();
                
                _expectations.Add(result);
            }
            if (count >= 3)
            {
                var builder = new FluentExpectationBuilder();
                var result = builder
                    .OnHandling(HttpMethod.Post)
                    .RespondWith(200, response => response.WithBody("body"))
                    .Setup()
                    .Expectations.First();
                
                _expectations.Add(result);
            }
            if (count >= 4)
            {
                var builder = new FluentExpectationBuilder();
                var result = builder
                    .OnHandling(HttpMethod.Post)
                    .RespondOnce(HttpStatusCode.BadGateway)
                    .And
                    .OnHandlingAny(HttpMethod.Get)
                    .RespondWith(404)
                    .Setup()
                    .Expectations.First();
                
                _expectations.Add(result);
            }
        }
        
    }
    
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
