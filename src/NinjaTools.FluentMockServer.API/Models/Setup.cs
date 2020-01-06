using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace NinjaTools.FluentMockServer.API.Models
{
  [DebuggerDisplay("{" + nameof(Id) + "} ({" + nameof(RequestMatcher) + "})" )]
    public class Setup
    {
        public Setup(Guid id) => Id = id;

        public Setup() : this(Guid.NewGuid())
        { }

        public Setup([NotNull] string id) : this (Guid.Parse(id))
        { }

        [NotNull]
        public Guid? Id { get; }

        [CanBeNull]
        public RequestMatcher? Matcher { get; set; }

        [CanBeNull]
        public ResponseAction? Action { get; set; }
    }
}
