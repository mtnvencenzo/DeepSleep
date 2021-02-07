namespace DeepSleep.OpenApi
{
    using System.Text.Json;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Text.Json.JsonNamingPolicy" />
    public class OasDefaultNamingPolicy : JsonNamingPolicy
    {
        /// <summary>When overridden in a derived class, converts the specified name according to the policy.</summary>
        /// <param name="name">The name to convert.</param>
        /// <returns>The converted name.</returns>
        public override string ConvertName(string name)
        {
            return name;
        }
    }
}
