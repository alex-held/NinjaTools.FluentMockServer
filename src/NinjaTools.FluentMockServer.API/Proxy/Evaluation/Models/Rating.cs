using System.Diagnostics;
using NinjaTools.FluentMockServer.API.Proxy.Evaluation.Evaluators;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Models
{
    [DebuggerDisplay("{PropertyName} {Score} | Points={Points}; Weight={Weight};", Name = "Rating")]
    public struct Rating
    {
        public Rating(uint points, EvaluationWeight weight, string propertyName)
        {
            PropertyName = propertyName;
            Points = points;
            Weight = weight;
        }

        public uint Points { get; }
        public EvaluationWeight Weight { get; }
        public uint Score => Points * (ushort) Weight;
        public string PropertyName { get; }
    }
}
