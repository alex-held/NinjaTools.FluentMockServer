using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Utils;


namespace NinjaTools.FluentMockServer.Models.HttpEntities
{

public class HttpTemplate 
    {
        public string Template { get; set; }

        public Delay Delay { get; set; }

       
    }
}
