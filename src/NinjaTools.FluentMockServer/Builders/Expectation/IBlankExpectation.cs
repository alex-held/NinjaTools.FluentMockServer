using System;
using System.ComponentModel;
using System.Net.Http;

using JetBrains.Annotations;

using NinjaTools.FluentMockServer.FluentInterfaces;


namespace NinjaTools.FluentMockServer.Builders
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IBlankExpectation : IFluentInterface
    {
        IWithRequest OnHandling([NotNull] HttpMethod method, [CanBeNull] Action<IFluentHttpRequestBuilder> requestFactory = null);
        IWithRequest OnHandlingAny([CanBeNull] HttpMethod method = null);
    }
}
