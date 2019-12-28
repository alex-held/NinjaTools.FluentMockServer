using System;

namespace NinjaTools.FluentMockServer.API.Models
{
    public class Setup
    {
        public Setup(Guid id) 
        {
            Id = id;
        }

        public Setup() : this(Guid.NewGuid())
        { }

        public Setup(string id) : this (Guid.Parse(id))
        { }


        public Guid? Id { get; }
        public RequestMatcher Matcher { get; set; }
        public ResponseAction Action { get; set; }
    }
}
