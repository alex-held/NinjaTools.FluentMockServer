namespace NinjaTools.FluentMockServer.API.Models
{
    public class Setup
    {
        public RequestMatcher Matcher { get; set; }

        public ResponseAction? Action { get; set; }
    }
}
