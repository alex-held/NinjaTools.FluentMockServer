using HardCoded.MockServer.Contracts.Abstractions;

namespace HardCoded.MockServer.Contracts.FluentInterfaces
{
    public interface IFluentBuilder<TBuildable> : IFluentInterface where TBuildable : IBuildable
    {
        TBuildable Build();
    }
}