using System;
using FluentApi.Generics.Framework;
using HardCoded.MockServer.Fluent.Framework;
using HardCoded.MockServer.Fluent.Models;

namespace HardCoded.MockServer.Fluent.Requests
{
    public interface IHandleRequests : IAttachable<IBuildRequests>
    {
        IApplyable RespondingWith(HttpResponse response);
        IApplyable RespondingWith(Func<HttpResponse> responseFactory);
        ICustomizable Delay(int seconds);
        
    }

    public interface ICustomizable : IAttachable<IBuildRequests>, IAttachable<IHandleRequests>
    {
        
    }
}