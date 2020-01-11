using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NinjaTools.FluentMockServer.API.Models.Logging;
using NinjaTools.FluentMockServer.API.Services;

namespace NinjaTools.FluentMockServer.API.Controllers
{
    [Route("log")]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet("list")]
        public Task<IEnumerable<ILogItem>> List([FromQuery] string? type)
        {
            if (Enum.TryParse<LogType>(type, out var logType))
            {
                var logs = _logService.OfType(logType);
                return Task.FromResult(logs);
            }
            else
            {
                var logs = _logService.Get();
                return Task.FromResult(logs);
            }
        }

        [HttpGet("matched")]
        public Task<IEnumerable<ILogItem>> GetMatchedRequests()
        {
            var logs = _logService.OfType<RequestMatchedLog>();
            return Task.FromResult<IEnumerable<ILogItem>>(logs);
        }

        [HttpGet("unmatched")]
        public Task<IEnumerable<ILogItem>> GetUnmatchedRequests()
        {
            var logs = _logService.OfType<RequestUnmatchedLog>();
            return Task.FromResult<IEnumerable<ILogItem>>(logs);
        }

        [HttpPost("prune")]
        public Task<IEnumerable<ILogItem>> Prune()
        {
            var logs = _logService.Prune();
            return Task.FromResult(logs);
        }
    }
}
