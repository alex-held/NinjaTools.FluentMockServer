namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Models
{
    public class EvaluationUnsuccessfulResult : EvaluationResultBase
    {
        /// <inheritdoc />
        public override bool IsMatch => false;

        /// <inheritdoc />
        public EvaluationUnsuccessfulResult(EvaluationContext context) : base(context)
        {
        }
    }
}
