namespace NinjaTools.FluentMockServer.API.Configuration
{
    internal static class MockServerConstants
    {
      
    }
    
    internal static class MockServerHttpContextKeys
    {
        private const string Prefix = "mockserver-key";
        public const string RequestId = Prefix  + nameof(RequestId);
        public const string PreviousRequestId = Prefix + nameof(PreviousRequestId);
    }
}
