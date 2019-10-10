using System.ComponentModel;
using FluentApi.Generics.Framework;

namespace HardCoded.MockServer.Fluent.Builder
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentConnectionOptionsBuilder : IFluentInterface
    {
        ConnectionOptions Build();
    }
}