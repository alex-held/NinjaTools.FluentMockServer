using System.Threading;

namespace NinjaTools.FluentMockServer.API.Proxy.Visitors
{
    public interface IVisitor
    { }

    public interface IVisitor<in TVisitable>
    {
        int Visit(TVisitable visitable, CancellationToken token = default);
    }
}
