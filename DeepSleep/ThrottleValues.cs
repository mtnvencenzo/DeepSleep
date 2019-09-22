using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public class ThrottleValues
    {
        /// <summary>
        /// Gets or sets the resets on.
        /// </summary>
        /// <value>
        /// The resets on.
        /// </value>
        public DateTime? ResetsOn { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public ulong? Count { get; set; }
    }
}
