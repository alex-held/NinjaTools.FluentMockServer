using System.Diagnostics;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Models
{
    /// <inheritdoc />
    [DebuggerDisplay("IsMatch={IsMatch}; Score={Ratings}; Errors={ErrorCount}; Messages={Messages};")]
    public class EvaluationSuccessfulResult : EvaluationResultBase
    {
        /// <inheritdoc />
        public EvaluationSuccessfulResult(EvaluationContext context) : base(context)
        {
        }

        /// <inheritdoc />
        public override bool IsMatch => true;
    }
}
