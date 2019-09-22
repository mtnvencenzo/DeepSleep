using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiRouteReflectionLocator
    {
        /// <summary>Initializes a new instance of the <see cref="ApiRouteReflectionLocator" /> class.</summary>
        /// <param name="directory">The directory.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <param name="searchOption">The search option.</param>
        public ApiRouteReflectionLocator(string directory, string searchPattern, SearchOption searchOption)
        {
            AssemblyDirectory = directory;
            SearchPattern = searchPattern;
            SearchOption = searchOption;
        }

        /// <summary>Gets the assembly directory.</summary>
        /// <value>The assembly directory.</value>
        public string AssemblyDirectory { get; private set; }

        /// <summary>Gets the search pattern.</summary>
        /// <value>The search pattern.</value>
        public string SearchPattern { get; private set; }

        /// <summary>Gets or sets the search option.</summary>
        /// <value>The search option.</value>
        public SearchOption SearchOption { get; private set; }

        /// <summary>Finds the API routes.</summary>
        /// <returns></returns>
        public IEnumerable<ApiRouteMappingInfo> FindApiRoutes()
        {
            var routes = new List<ApiRouteMappingInfo>();
            var files = Directory.GetFiles(AssemblyDirectory, SearchPattern, SearchOption);

            foreach (var file in files)
            {
                try
                {
                    var assembly = Assembly.Load(File.ReadAllBytes(file));

                    var types = assembly.GetTypes()
                        .Where(t => t.IsClass)
                        .Where(t => t.IsPublic);

                    foreach (var type in types)
                    {
                        try
                        {
                            var methods = type.GetMethods()
                                .Where(m => m.GetCustomAttribute<ApiRouteAttribute>(false) != null);

                            if (methods.Count() > 0)
                            {
                                foreach (var method in methods)
                                {
                                    try
                                    {
                                        var attribute = method.GetCustomAttribute<ApiRouteAttribute>(false);

                                        if (attribute != null)
                                        {
                                            routes.Add(new ApiRouteMappingInfo
                                            {
                                                ContrallerType = type.AssemblyQualifiedName,
                                                MethodName = method.Name,
                                                HttpMethod = attribute.HttpMethod,
                                                Route = attribute.Route
                                            });
                                        }
                                    }
                                    catch (System.Exception) { }
                                }
                            }
                        }
                        catch (System.Exception) { }
                    }
                }
                catch (System.Exception) { }
            }

            return routes;
        }
    }
}
