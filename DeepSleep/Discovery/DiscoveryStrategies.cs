namespace DeepSleep.Discovery
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// 
    /// </summary>
    public static class DiscoveryStrategies
    {
        /// <summary>Defaults this instance.</summary>
        /// <returns></returns>
        public static IList<IDeepSleepDiscoveryStrategy> Default()
        {
            var assemblyDirectoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            return new List<IDeepSleepDiscoveryStrategy>
            {
                new AttributeRouteDiscoveryStrategy(assemblyDirectoryPath: assemblyDirectoryPath, assemblyMatchPattern: "*"),
                new DelegatedRouteDiscoveryStrategy(assemblyDirectoryPath: assemblyDirectoryPath, assemblyMatchPattern: "*")
            };
        }
    }
}
