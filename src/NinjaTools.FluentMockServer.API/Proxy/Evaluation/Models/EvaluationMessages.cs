using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Models
{
    [DebuggerDisplay("[M: {MessageCount} E: {ErrorCount} ]  |  Messages={Messages}; Errors={Exceptions};", Name = "EvaluatingMessages")]
    public class EvaluationMessages
    {
        public int ErrorCount => Exceptions.Count;
        public int MessageCount => Messages.Count;
        public List<Exception> Exceptions { get; }
        public List<string> Messages { get; }

        public EvaluationMessages()
        {
            Exceptions = new List<Exception>();
            Messages = new List<string>();
        }
    }
}