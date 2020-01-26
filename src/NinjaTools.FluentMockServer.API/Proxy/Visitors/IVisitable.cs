namespace NinjaTools.FluentMockServer.API.Proxy.Visitors
{
    public interface IVisitable
    {
        void Accept(IVisitor visitor);
    }
}