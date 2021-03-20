namespace DeepSleep.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public static class ApiPaths
    {
        /// <summary>Froms the root.</summary>
        /// <param name="rootPath">The root path.</param>
        /// <returns></returns>
        public static string FromRoot(string rootPath)
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
