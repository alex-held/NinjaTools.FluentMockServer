using JetBrains.Annotations;

namespace NinjaTools.FluentMockServer.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Indicates that a connection to the MockServer could not be established or an operation failed.
    /// </summary>
    [PublicAPI]
    public sealed class MockServerOperationFailedException : MockServerException
    {
        /// <summary>
        /// The failed operation.
        /// </summary>
        public string Operation { get; }

        /// <inheritdoc />
        public MockServerOperationFailedException(string operation)
        {
            Operation = operation;
        }
    }
}