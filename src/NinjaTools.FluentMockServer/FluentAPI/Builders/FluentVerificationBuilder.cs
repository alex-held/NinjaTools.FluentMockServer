using System;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.FluentAPI.Builders.HttpEntities;
using NinjaTools.FluentMockServer.Models;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.FluentAPI.Builders
{
    internal class FluentVerificationBuilder : IFluentVerificationBuilder, IFluentVerificationBuilder.IWithRequest
    {
        private HttpRequest _httpRequest;
        private VerificationTimes _verificationTimes;
            

        /// <inheritdoc />
        [NotNull]
        public Verify Build()
        {
            return new Verify(_httpRequest, _verificationTimes);
        }


        /// <inheritdoc />
        [NotNull]
        public IFluentVerificationBuilder.IWithRequest  Verify([NotNull] Action<IFluentHttpRequestBuilder> request)
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
