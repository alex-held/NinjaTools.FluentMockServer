using System;
using System.ComponentModel;
using System.Net.Http;
using HardCoded.MockServer.Contracts.FluentInterfaces;
using HardCoded.MockServer.Fluent.Builder.Request;

using JetBrains.Annotations;


namespace HardCoded.MockServer.Fluent.Builder.Expectation
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IBlankExpectation : IFluentInterface
    {
        IWithRequest OnHandling(HttpMethod method, Action<IFluentHttpRequestBuilder> requestFactory);
    }
}