using JetBrains.Annotations;

namespace NinjaTools.FluentMockServer.FluentAPI
{
    [PublicAPI]
    public interface IFluentBuilder<out TBuildable> : IFluentInterface where TBuildable : class
    {
        TBuildable Build();
    }
}
