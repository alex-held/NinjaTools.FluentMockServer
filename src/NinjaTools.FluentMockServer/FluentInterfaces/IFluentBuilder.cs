using NinjaTools.FluentMockServer.Abstractions;


namespace NinjaTools.FluentMockServer.FluentInterfaces
{
    public interface IFluentBuilder<out TBuildable> : IFluentInterface where TBuildable : IBuildable
    {
        TBuildable Build();
    }
}
