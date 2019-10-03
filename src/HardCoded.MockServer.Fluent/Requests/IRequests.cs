using FluentApi.Generics.Framework;
using HardCoded.MockServer.Fluent.Framework;

namespace HardCoded.MockServer.Fluent.Requests
{
    public interface IRequests : IApplyable, ISupportsSelect<IBuildRequests>
    {
    }
}