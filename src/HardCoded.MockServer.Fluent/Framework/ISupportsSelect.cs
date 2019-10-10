using System;
using HardCoded.MockServer.Models.HttpEntities;

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
