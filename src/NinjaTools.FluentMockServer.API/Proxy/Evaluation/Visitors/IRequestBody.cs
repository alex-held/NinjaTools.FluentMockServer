using System;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Visitors
{
    public interface IRequestBody
    {
        void Accept(Func<IPartialVisitor> visitorFactory);
    }
}
