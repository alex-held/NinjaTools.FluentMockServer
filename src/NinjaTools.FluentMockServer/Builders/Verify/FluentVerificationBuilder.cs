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
            void AtLeast(int value);
            void AtMost(int value);
            void Between(int min, int max);
            void Once();
            void Twice();
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
        public void AtLeast(int value) => _verify.Times = VerficationTimes.MoreThan(value);
        
        
        /// <inheritdoc />
        public void AtMost(int value) => _verify.Times = VerficationTimes.LessThan(value);


        /// <inheritdoc />
        public void Between(int min, int max) => _verify.Times = VerficationTimes.Between(min, max);


        /// <inheritdoc />
        public void Once() => _verify.Times = VerficationTimes.Once;
        
        
        /// <inheritdoc />
        public void Twice() => _verify.Times = VerficationTimes.Twice;
    }
}
