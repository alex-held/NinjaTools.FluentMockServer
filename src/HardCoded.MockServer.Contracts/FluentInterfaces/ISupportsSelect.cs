using System;

namespace HardCoded.MockServer.Contracts.FluentInterfaces
{
    public interface ISupportsSelect<T> 
    {
        T Select();
        T OnSelect<P>(Func<P, bool> func);
        T OnHandling<P>(Func<P> requestFactory);
        
    }
}
