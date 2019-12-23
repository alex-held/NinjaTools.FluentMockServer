using JetBrains.Annotations;

namespace NinjaTools.FluentMockServer.API.Infrastructure
{
    public class RequestId
    {
        public RequestId([NotNull] string requestIdKey, [NotNull] string requestIdValue)
        {
            RequestIdKey = requestIdKey;
            RequestIdValue = requestIdValue;
        }

        [NotNull]
        public string RequestIdKey { get; private set; }
        
        [NotNull]
        public string RequestIdValue { get; private set; }
    }
}
