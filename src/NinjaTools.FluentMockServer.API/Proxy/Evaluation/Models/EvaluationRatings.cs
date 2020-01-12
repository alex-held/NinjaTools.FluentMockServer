using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Models
{
    [DebuggerDisplay("[{Score}] | EvaluationScore={WeightedEvaluationPoints}; BonusPoints={BonusPoints};")]
    public class EvaluationRatings : List<Rating>
    {
        public long Score => WeightedEvaluationPoints + BonusPoints;
        public long WeightedEvaluationPoints => this.Sum(rating => rating.Score);
        public int BonusPoints { get; internal set; }
    }
}
