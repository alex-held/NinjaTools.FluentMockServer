using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NinjaTools.FluentMockServer.API.Models.Logging;
using NinjaTools.FluentMockServer.API.Services;

namespace NinjaTools.FluentMockServer.API.Controllers
{
    [Route("log")]
    [Produces("application/json")]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;

        /// <inheritdoc />
        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        /// <summary>
        /// Gets all logs available.
        /// </summary>
        /// <param name="type">Request, Setup</param>
        /// <returns>A list of <see cref="ILogItem"/> matching on your query.</returns>
        [HttpGet("list")]
        [ProducesResponseType(typeof(IEnumerable<ILogItem>), 200)]
        public Task<IEnumerable<ILogItem>> List([FromQuery] LogType? type)
        {
            if (type.HasValue)
            {
                var logs = _logService.OfType(type.Value);
                return Task.FromResult(logs);
            }
            else
            {
                var logs = _logService.Get();
                return Task.FromResult(logs);
            }
        }

        /// <summary>
        /// Resets all logs.
        /// </summary>
        /// <returns>A list of <see cref="ILogItem"/> which have been deleted.</returns>
        [HttpGet("prune")]
        [ProducesResponseType(typeof(IEnumerable<ILogItem>), 200)]
        public Task<IEnumerable<ILogItem>> Prune([FromQuery] LogType? type)
        {
            if (type.HasValue)
            {
                var logs = _logService.Prune(type.Value);
                return Task.FromResult(logs);
            }
            else
            {
                var logs = _logService.Prune();
                return Task.FromResult(logs);
            }
        }

        /// <summary>
        /// Gets all <see cref="RequestMatchedLog"/>.
        /// </summary>
        /// <returns>A list of <see cref="RequestMatchedLog"/>.</returns>
        [HttpGet("matched")]
        [ProducesResponseType(typeof(IEnumerable<ILogItem>), 200)]
        public Task<IEnumerable<ILogItem>> GetMatchedRequests()
        {
            var logs = _logService.OfType<RequestMatchedLog>();
            return Task.FromResult<IEnumerable<ILogItem>>(logs);
        }

        /// <summary>
        /// Gets all <see cref="RequestUnmatchedLog"/>.
        /// </summary>
        /// <returns>A list of <see cref="RequestUnmatchedLog"/>.</returns>
        [HttpGet("unmatched")]
        [ProducesResponseType(typeof(IEnumerable<ILogItem>), 200)]
        public Task<IEnumerable<ILogItem>> GetUnmatchedRequests()
        {
            var logs = _logService.OfType<RequestUnmatchedLog>();
            return Task.FromResult<IEnumerable<ILogItem>>(logs);
        }


    }
}
