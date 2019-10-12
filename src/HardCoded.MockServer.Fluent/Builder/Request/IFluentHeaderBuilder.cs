using HardCoded.MockServer.Contracts.FluentInterfaces;

namespace HardCoded.MockServer.Fluent.Builder.Request
{
    public interface IFluentHeaderBuilder : IFluentInterface
    {
        IFluentHeaderBuilder WithHeaders(params (string name, string value)[] headers);
        IFluentHeaderBuilder AddHeader(string name, string value);
    }
}