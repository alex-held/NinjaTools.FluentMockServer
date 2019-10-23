using System;

using HardCoded.MockServer.Builder.Request;
using HardCoded.MockServer.Contracts.Models.ValueTypes;


namespace HardCoded.MockServer.Builder.Verify
{
    public class FluentVerificationBuilder : IFluentVerificationBuilder, FluentVerificationBuilder.IWithRequest
    {
        private Contracts.Models.Verify _verify;


        public FluentVerificationBuilder()
        {
            _verify = new Contracts.Models.Verify();
        }
         
        public interface IWithRequest
        {
            void Times(int value);
            void Always();
        }


        /// <inheritdoc />
        public Contracts.Models.Verify Build() => _verify;


        /// <inheritdoc />
        public IWithRequest Verify(Action<IFluentHttpRequestBuilder> request)
        {
            var builder = new FluentHttpRequestBuilder();
            request(builder);
            _verify.HttpRequest = builder.Build();

            return this;
        }


        /// <inheritdoc />
        public void Times(int value) => _verify.Times = new Times(value);
        
        /// <inheritdoc />
        public void Always() => _verify.Times = Contracts.Models.ValueTypes.Times.Always; 
    }
}
