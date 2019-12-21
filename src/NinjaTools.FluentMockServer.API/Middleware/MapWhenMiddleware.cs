using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Configuration;

namespace NinjaTools.FluentMockServer.API.Middleware
{
    public class MapWhenOptions
    {
        private Func<DownstreamContext, bool> _predicate;

        public Func<DownstreamContext, bool> Predicate
        {
            [DebuggerStepThrough] get => _predicate;
            [DebuggerStepThrough]
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                
                _predicate = value;
            }
        }
        
        public MockServerRequestDelegate Branch { get; set; } 
    }
    public class MapWhenMiddleware
    {
        private readonly MockServerRequestDelegate _next;
        private readonly MapWhenOptions _options;

        public MapWhenMiddleware(MockServerRequestDelegate next, MapWhenOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(DownstreamContext context)
        {
            if (_options.Predicate.Invoke(context))
            {
                await _options.Branch(context);
            }
            else
            {
                await _next(context);
            }
        }
    }
}
