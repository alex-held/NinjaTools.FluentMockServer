using System;
using HardCoded.MockServer.Fluent.Builder;
using HardCoded.MockServer.Fluent.Models;
using HardCoded.MockServer.Fluent.Requests;

namespace FluentApi.Generics.Framework
{
    public interface ISupportsSelect<T> 
    {
        T Select();
        T OnSelect<P>(Func<P, bool> func);
      //  T OnHandling<TBuilder>(Func<TBuilder, HttpRequest> requestFactory);
        T OnHandling(Func<HttpRequest> requestFactory);
        
    }
}
