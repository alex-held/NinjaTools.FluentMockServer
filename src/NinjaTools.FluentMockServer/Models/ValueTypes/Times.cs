namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    public class Times
    {
        public Times()
        {
        }

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

        public int RemainingTimes { get; set; }

        public bool Unlimited { get; set; }

        public static Times Once => new Times(1);
        public static Times Always => new Times(0);
    }
}
