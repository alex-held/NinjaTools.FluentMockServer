namespace NinjaTools.FluentMockServer.API.Types
{
    public interface IDontRenderWhenEmpty
    {
        /// <summary>
        /// Displays whether of not that component will be serialized.
        /// </summary>
        /// <returns></returns>
        bool IsEnabled();
    }
}
