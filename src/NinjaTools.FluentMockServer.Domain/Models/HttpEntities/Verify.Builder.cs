using System;
using NinjaTools.FluentMockServer.Domain.Builders.Request;
using NinjaTools.FluentMockServer.Domain.FluentInterfaces;
using NinjaTools.FluentMockServer.Domain.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.Domain.Models.HttpEntities
{
    public interface IFluentVerificationBuilder : IFluentBuilder<Domain.Models.HttpEntities.Verify>
    {
        Verify.FluentVerificationBuilder.IWithRequest Verify(Action<IFluentHttpRequestBuilder> request);
    }
    
    public partial class Verify
    {
        public class FluentVerificationBuilder : IFluentVerificationBuilder, FluentVerificationBuilder.IWithRequest
        {
            public interface IWithRequest
            {
                void AtLeast(int value);
                void AtMost(int value);
                void Between(int min, int max);
                void Once();
                void Twice();
            }


            private HttpRequest _httpRequest;
            private VerificationTimes _verificationTimes;
            

            /// <inheritdoc />
            public Verify Build()
            {
                return new Verify(_httpRequest, _verificationTimes);
            }


            /// <inheritdoc />
            public IWithRequest  Verify(Action<IFluentHttpRequestBuilder> request)
            {
                var builder = new FluentHttpRequestBuilder();
                request(builder);
                _httpRequest = builder.Build();
                return this;
            }


            /// <inheritdoc />
            public void AtLeast(int value)
            {
                _verificationTimes = VerificationTimes.MoreThan(value);
            }
            
            /// <inheritdoc />
            public void AtMost(int value)
            {
                _verificationTimes = VerificationTimes.LessThan(value);
            }


            /// <inheritdoc />
            public void Between(int min, int max)
            {
                _verificationTimes = VerificationTimes.Between(min, max);
            }


            /// <inheritdoc />
            public void Once()
            {
                _verificationTimes = VerificationTimes.Once;
            }


            /// <inheritdoc />
            public void Twice()
            {
                _verificationTimes = VerificationTimes.Twice;
            }
        }
    }
}
