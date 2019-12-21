using System;

namespace NinjaTools.FluentMockServer.API.Logging
{
    public interface IMockServerLogger
    {
        void LogTrace(string message);
        void LogDebug(string message);
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message, Exception exception);
        void LogCritical(string message, Exception exception);
    }
}
