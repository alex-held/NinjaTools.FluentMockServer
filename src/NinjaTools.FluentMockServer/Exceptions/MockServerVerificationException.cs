using System;
using NinjaTools.FluentMockServer.Models.HttpEntities;

namespace NinjaTools.FluentMockServer.Exceptions
{
    public class MockServerException : Exception
    {
    }

    public class MockServerOperationFailedException : Exception
    {
        public string Operation { get; }

        /// <inheritdoc />
        public MockServerOperationFailedException(string operation)
        {
            Operation = operation;
        }
    }

    public class MockServerVerificationException : MockServerException
    {
        public string ResponseMessage { get; }
        public HttpRequest Expected { get; }

        /// <inheritdoc />
        public MockServerVerificationException(string responseMessage, HttpRequest expectedHttpRequest)
        {
            ResponseMessage = responseMessage;
            Expected = expectedHttpRequest;
        }
    }
}
