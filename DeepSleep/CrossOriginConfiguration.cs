namespace DeepSleep
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class CrossOriginConfiguration
    {
        /// <summary>Gets or sets the expose headers.</summary>
        /// <value>The expose headers.</value>
        public IList<string> ExposeHeaders { get; set; }

        /// <summary>Gets or sets the allowed origins.</summary>
        /// <value>The allowed origins.</value>
        public IList<string> AllowedOrigins { get; set; }

        /// <summary>Gets or sets a value indicating whether [allow credentials].</summary>
        /// <value><c>true</c> if [allow credentials]; otherwise, <c>false</c>.</value>
        public bool? AllowCredentials { get; set; }

        /// <summary>Gets or sets the allowed headers.</summary>
        /// <value>The allowed headers.</value>
        public IList<string> AllowedHeaders { get; set; }
    }
}
