namespace NinjaTools.FluentMockServer.API.Logging
{
    public interface IMockServerLoggerFactory
    {
        IMockServerLogger CreateLogger<T>();
    }
}
