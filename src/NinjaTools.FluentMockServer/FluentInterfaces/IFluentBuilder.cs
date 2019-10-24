using NinjaTools.FluentMockServer.Abstractions;


namespace NinjaTools.FluentMockServer.FluentInterfaces
{
    public interface IFluentBuilder<TBuildable> : IFluentInterface where TBuildable : IBuildable
    {
        TBuildable Build();
    }
}