using System;

using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;


namespace NinjaTools.FluentMockServer.Builders
{
    public class FluentVerificationBuilder : IFluentVerificationBuilder, FluentVerificationBuilder.IWithRequest
    {
        private Verify _verify;


        public FluentVerificationBuilder()
        {
            _verify = new Verify();;
        }
         
        public interface IWithRequest
        {
            void VerifyTimes(int value);
            void Always();
        }


        /// <inheritdoc />
        public Verify Build() => _verify;


        /// <inheritdoc />
        public IWithRequest Verify(Action<IFluentHttpRequestBuilder> request)
        {
            var builder = new FluentHttpRequestBuilder();
            request(builder);
            _verify.HttpRequest = builder.Build();

            return this;
        }


        /// <inheritdoc />
        public void VerifyTimes(int value) => _verify.Times = new Times(value);


        /// <inheritdoc />
        public void Always() => _verify.Times = Times.Always;
    }
}
