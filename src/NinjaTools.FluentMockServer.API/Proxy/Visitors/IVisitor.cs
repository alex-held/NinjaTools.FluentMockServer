using System.Threading;

namespace NinjaTools.FluentMockServer.API.Proxy.Visitors
{
    public interface IVisitor
    { }

    public interface IVisitor<in TVisitable>
    {
        void Visit(TVisitable visitable) => Visit(visitable,default);
        double Visit(TVisitable visitable, CancellationToken token);
    }
}
