namespace NinjaTools.FluentMockServer.FluentAPI
{
    public interface IFluentBuilder<out TBuildable> : IFluentInterface where TBuildable : class
    {
        TBuildable Build();
    }
}
