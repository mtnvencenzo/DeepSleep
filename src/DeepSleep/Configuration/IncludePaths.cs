namespace DeepSleep.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public static class IncludePaths
    {
        /// <summary>Roots the regex.</summary>
        /// <param name="rootPath">The root path.</param>
        /// <returns></returns>
        public static string RootRegex(string rootPath)
        {
            var root = rootPath
                .TrimStart('/')
                .TrimEnd('/')
                .Trim();

            return $@"^/?{root}(/)?(?(1).*|)$";
        }

        /// <summary>Alls this instance.</summary>
        /// <returns></returns>
        public static string All()
        {
            return "*";
        }
    }
}
