using System.Linq;
using System.Net.Http;
using JetBrains.Annotations;

public static class HttpExtensions
{
    public const string MockContextHeaderKey = "X-MockServer-Context";

    public static bool TryGetMockContextHeader([NotNull] this HttpRequestMessage message, out string value)
    {
        if (message.Headers.TryGetValues(MockContextHeaderKey, out var values))
        {
            value =  values.First();
            return true;
        }

        value = null;
        return false;
    }
}
