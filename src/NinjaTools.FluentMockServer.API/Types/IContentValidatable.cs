namespace NinjaTools.FluentMockServer.API.Types
{
    public interface IContentValidatable
    {
        /// <summary>
        /// Displays whether of not that component will be serialized.
        /// </summary>
        /// <returns></returns>
        bool HasContent();
    }
}
