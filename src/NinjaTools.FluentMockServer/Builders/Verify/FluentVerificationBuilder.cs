using System;
using NinjaTools.FluentMockServer.Builders.Request;
using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.Builders.Verify
{
    public class FluentVerificationBuilder : IFluentVerificationBuilder, FluentVerificationBuilder.IWithRequest
    {
        private readonly Models.HttpEntities.Verify _verify;

        public FluentVerificationBuilder()
        {
            _verify = new Models.HttpEntities.Verify();
            ;
        }


        /// <inheritdoc />
        public Models.HttpEntities.Verify Build()
        {
            return _verify;
        }


        /// <inheritdoc />
        public IWithRequest Verify(Action<IFluentHttpRequestBuilder> request)
        {
            var builder = new FluentHttpRequestBuilder();
            request(builder);
            _verify.HttpRequest = builder.Build();

            return this;
        }


        /// <inheritdoc />
        public void AtLeast(int value)
        {
            _verify.Times = VerficationTimes.MoreThan(value);
        }


        /// <inheritdoc />
        public void AtMost(int value)
        {
            _verify.Times = VerficationTimes.LessThan(value);
        }


        /// <inheritdoc />
        public void Between(int min, int max)
        {
            _verify.Times = VerficationTimes.Between(min, max);
        }


        /// <inheritdoc />
        public void Once()
        {
            _verify.Times = VerficationTimes.Once;
        }


        /// <inheritdoc />
        public void Twice()
        {
            _verify.Times = VerficationTimes.Twice;
        }

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
