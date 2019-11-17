namespace NinjaTools.FluentMockServer.FluentInterfaces
{
    public interface IFluentBuilder<out TBuildable> : IFluentInterface where TBuildable : class
    {
        TBuildable Build();
    }
}
