

namespace NinjaTools.FluentMockServer.Serialization {
    using Newtonsoft.Json.Serialization;


    /// <summary>
    /// Property names and dictionary keys will be UPPERCASE.
    /// </summary>
    public class UpperCaseNamingStrategy : NamingStrategy
    {
        /// <summary>
        /// Resolves the specified property name.
        /// </summary>
        /// <param name="name">The property name to resolve.</param>
        /// <returns>The resolved property name.</returns>
        protected override string ResolvePropertyName(string name)
        {
            return name.ToUpper();
        }

        public UpperCaseNamingStrategy()
        {
            this.OverrideSpecifiedNames = true;
        }
    }
}
