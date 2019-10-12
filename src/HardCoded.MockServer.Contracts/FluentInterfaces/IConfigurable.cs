using System;

namespace HardCoded.MockServer.Contracts.FluentInterfaces
{
    public interface IConfigurable<T> where T : IConfigurable<T>
    {
        T Delay(int ticks);
        T Callback(Action callback);
    }
}