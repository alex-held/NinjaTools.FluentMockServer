using System;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.FluentAPI.Builders.HttpEntities;
using NinjaTools.FluentMockServer.Models;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.FluentAPI.Builders
{
    internal class FluentVerificationBuilder : IFluentVerificationBuilder, IWithVerify
    {
        private HttpRequest? _httpRequest;
        private VerificationTimes? _verificationTimes;
        private MockContext? _mockContext;

        /// <inheritdoc />
        public Verify Build()
        {
            if (_mockContext != null)
            {
                _httpRequest = _mockContext.Apply(_httpRequest);
            }

            return new Verify(_httpRequest, _verificationTimes);
        }


        /// <inheritdoc />
        public IWithVerify  Verify([NotNull] Action<IFluentHttpRequestBuilder> request)
        {
            var builder = new FluentHttpRequestBuilder();
            request(builder);
            _httpRequest = builder.Build();
            return this;
        }

        /// <inheritdoc />
        public IWithVerify VerifyInContext(string context, Action<IFluentHttpRequestBuilder> request)
        {
            _mockContext = new MockContext(context);
            return Verify(request);
        }


        /// <inheritdoc />
        public void AtLeast(int value)
        {
            _verificationTimes = VerificationTimes.MoreThan(value);
        }

        /// <inheritdoc />
        public void AtLeastOnce()
        {
            _verificationTimes = VerificationTimes.MoreThan(0);
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

        /// <inheritdoc />
        public void Times(VerificationTimes times)
        {
            _verificationTimes = times;
        }
    }
  
}
