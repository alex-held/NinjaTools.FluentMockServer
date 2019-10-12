using System.ComponentModel;
using HardCoded.MockServer.Contracts.FluentInterfaces;
using HardCoded.MockServer.Models;

namespace HardCoded.MockServer.Fluent.Builder.Response
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentConnectionOptionsBuilder : IFluentBuilder<ConnectionOptions>, IFluentInterface
    {
        ConnectionOptions Build();
    }
}