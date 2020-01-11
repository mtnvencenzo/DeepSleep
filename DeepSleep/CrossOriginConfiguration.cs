namespace DeepSleep
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class CrossOriginConfiguration
    {
        /// <summary>Initializes a new instance of the <see cref="CrossOriginConfiguration"/> class.</summary>
        public CrossOriginConfiguration()
        {
            ExposeHeaders = new List<string>();
            AllowedOrigins = new List<string> { "*" };
        }

        /// <summary>Gets or sets the expose headers.</summary>
        /// <value>The expose headers.</value>
        public IEnumerable<string> ExposeHeaders { get; set; }

        /// <summary>Gets or sets the allowed origins.</summary>
        /// <value>The allowed origins.</value>
        public IEnumerable<string> AllowedOrigins { get; set; }
    }
}
