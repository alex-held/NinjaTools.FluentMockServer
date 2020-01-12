using System.Diagnostics;
using NinjaTools.FluentMockServer.API.Proxy.Evaluation.Evaluators;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Models
{
    [DebuggerDisplay("{HttpRequestMember} {Score} | Points={Points}; Weight={Weight};", Name = "Rating")]
    public class Rating
    {
        public Rating(uint points, EvaluationWeight weight)
        {
            Points = points;
            Weight = weight;
        }

        public uint Points { get; }
        public EvaluationWeight Weight { get; }
        public uint Score => Points * (ushort) Weight;


        public object? HttpRequestMember { get; set; }
        public object? Member { get; set; }
    }
}
