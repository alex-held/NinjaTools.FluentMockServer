namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Models
{
    public interface IEvaluationResult
    {
        long Score { get; }
        bool IsMatch { get; }
        EvaluationRatings Ratings { get; }
    }
}
