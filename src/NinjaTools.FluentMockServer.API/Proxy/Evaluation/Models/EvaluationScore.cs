using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using NinjaTools.FluentMockServer.API.Proxy.Evaluation.Evaluators;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Models
{
    [DebuggerDisplay("[{Score}] | EvaluationScore={WeightedEvaluationPoints}; BonusPoints={BonusPoints};")]
    public class EvaluationScore : List<Rating>
    {
        public uint Score => WeightedEvaluationPoints + BonusPoints;
        public uint WeightedEvaluationPoints => Convert.ToUInt32(this.ToList().Sum(r => r.Points * (ushort) r.Weight));
        public uint BonusPoints { get; internal set; }

        public void Add(EvaluationWeight weight, [CallerMemberName] string caller= null)
        {
            Add(new Rating(1, weight, caller));
        }
    }
}
