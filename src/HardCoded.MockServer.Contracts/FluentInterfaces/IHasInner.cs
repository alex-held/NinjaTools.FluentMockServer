namespace HardCoded.MockServer.Contracts.FluentInterfaces
{
    public interface IHasInner<T>
    {
        T Inner { get; }
    }
}