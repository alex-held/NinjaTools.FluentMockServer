using Newtonsoft.Json.Serialization;

namespace NinjaTools.FluentMockServer.Utils
{
    /// <inheritdoc />
    public class CustomNamingStrategy : CamelCaseNamingStrategy
    {
        /// <inheritdoc />
        public CustomNamingStrategy()
        {
            ProcessDictionaryKeys = true;
            OverrideSpecifiedNames = true;
        }   
    }
}