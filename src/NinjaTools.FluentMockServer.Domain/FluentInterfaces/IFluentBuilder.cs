namespace NinjaTools.FluentMockServer.Domain.FluentInterfaces
{
    public interface IFluentBuilder<out TBuildable> : IFluentInterface where TBuildable : class
    {
        TBuildable Build();
    }
}
