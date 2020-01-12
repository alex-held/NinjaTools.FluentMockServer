using System;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Logging.Models;
using NinjaTools.FluentMockServer.API.Models;

namespace NinjaTools.FluentMockServer.API.Logging
{
    public delegate string GenerateId();

    public interface ILogFactory
    {

        string GenerateId();
        SetupLog SetupCreated(Setup setup);
        SetupLog SetupDeleted(Setup setup);
        RequestMatchedLog RequestMached(HttpContext context, Setup setup);
        RequestUnmatchedLog RequestUnmatched(HttpContext context);
    }


    public class LogFactory
    {
        private readonly GenerateId _idGenerator;

        public LogFactory() : this(() => Guid.NewGuid().ToString())
        {
        }

        public LogFactory(GenerateId idGenerator)
        {
            _idGenerator = idGenerator;
        }

        public SetupLog SetupCreated(Setup setup) => new SetupLog(_idGenerator(), setup, LogKind.SetupCreated);
        public SetupLog SetupDeleted(Setup setup) => new SetupLog(_idGenerator(), setup, LogKind.SetupDeleted);
        public RequestMatchedLog RequestMached(HttpContext context, Setup setup) => new RequestMatchedLog(_idGenerator(), (context, setup));
        public RequestUnmatchedLog RequestUnmatched(HttpContext context) => new RequestUnmatchedLog(_idGenerator(), context);
    }
}
