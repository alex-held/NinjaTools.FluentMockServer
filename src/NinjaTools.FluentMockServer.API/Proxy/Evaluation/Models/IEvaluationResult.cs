using System;
using System.Collections.Generic;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Models
{
    public interface IEvaluationResult
    {
        uint Score { get; }
        bool IsMatch { get; }
        IReadOnlyList<string> Messages { get; }
        IReadOnlyList<Exception> Errors { get; }
        int ErrorCount { get; }
    }
}
