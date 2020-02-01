using System;
using NinjaTools.FluentMockServer.Models;
using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.FluentAPI
{
    public interface IFluentVerificationBuilder : IFluentBuilder<Verify>
    {
        IWithVerify Verify(Action<IFluentHttpRequestBuilder> request);

    }

    public interface IWithVerify
    {
        void AtLeast(int value);
        void AtLeastOnce();
        void AtMost(int value);
        void Between(int min, int max);
        void Once();
        void Twice();
        void Times(VerificationTimes times);
    }
}
