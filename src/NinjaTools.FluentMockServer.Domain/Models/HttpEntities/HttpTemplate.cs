using NinjaTools.FluentMockServer.Domain.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.Domain.Models.HttpEntities
{
    public partial class HttpTemplate
    {
        public HttpTemplate(string template, Delay delay)
        {
            Template = template;
            Delay = delay;
        }
        
        public string Template { get;  }

        public Delay Delay { get; }
    }
}
