using System;
namespace FluentApi.Generics.Framework
{

    public interface IHasInner<T>
    {
        T Inner { get; }
    }

  
    public class Wrapper<T> : IHasInner<T>
    {
        public Wrapper(T inner)
        {
            Inner = inner;
        }

        public virtual T Inner
        {
            get; private set;
        }

        public void SetInner(T inner)
        {
            Inner = inner;
        }
    }
    
    public interface IConfigurable<T> where T : IConfigurable<T>
    {
        T Delay(int ticks);
        T Callback(Action callback);
    }
    
    public interface IAttachable<T>
    {
        T Attach();
    }
}
