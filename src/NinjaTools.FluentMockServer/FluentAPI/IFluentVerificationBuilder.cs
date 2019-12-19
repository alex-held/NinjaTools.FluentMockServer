using System;
using NinjaTools.FluentMockServer.Models;

namespace NinjaTools.FluentMockServer.FluentAPI
{
    public interface IFluentVerificationBuilder : IFluentBuilder<Verify>
    {
        IWithRequest Verify(Action<IFluentHttpRequestBuilder> request);
        
        public interface IWithRequest
        {
            void AtLeast(int value);
            void AtMost(int value);
            void Between(int min, int max);
            void Once();
            void Twice();
        }
    }
}
