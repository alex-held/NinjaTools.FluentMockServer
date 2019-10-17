using HardCoded.MockServer.Contracts.Abstractions;

namespace HardCoded.MockServer.Models.ValueTypes
{
   
    public class Delay  : BuildableBase
    {
        public Delay()
        {
        }
        
        private Delay(int delay, TimeUnit unit)
        {
            TimeUnit = unit;
            Value = delay;
        }

        public static Delay FromSeconds(int seconds) => new Delay(seconds, TimeUnit.SECONDS);
        public static Delay FromMinutes(int minutes) => new Delay(minutes, TimeUnit.MINUTES);
        public static Delay None => new Delay();
        
        public TimeUnit TimeUnit { get; set; }

        public int Value { get; set; }
    }
}
