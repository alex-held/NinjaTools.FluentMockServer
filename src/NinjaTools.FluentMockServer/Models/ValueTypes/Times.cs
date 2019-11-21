using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    public class Times 
    {
        public int RemainingTimes { get; set; }

        public bool Unlimited { get; set; }
        
        public Times()
        { }

        public Times(int remainingTimes)
        {
            if (remainingTimes <= 0)
            {
                Unlimited = true;
            }
            else
            {
                RemainingTimes = remainingTimes;
                Unlimited = false;
            }
        }

        public static Times Once => new Times(1);
        public static Times Always => new Times(0);
    }
}
