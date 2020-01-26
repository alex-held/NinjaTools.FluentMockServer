using System.Threading;

namespace NinjaTools.FluentMockServer.API.Proxy.Visitors
{
    public interface IVisitor
    { }

    public interface IVisitor<TVisitable>
    {
        void Visit(TVisitable visitable) => Visit(visitable,default);
        double Visit(TVisitable visitable, CancellationToken token);
    }
}
