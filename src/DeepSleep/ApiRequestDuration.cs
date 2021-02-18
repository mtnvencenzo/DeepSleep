namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestDuration
    {
        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public DateTimeOffset UtcStart { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        public DateTimeOffset? UtcEnd { get; set; }

        /// <summary>Gets or sets the duration.</summary>
        /// <value>The duration.</value>
        public int Duration
        {
            get
            {
                if (UtcEnd.HasValue)
                {
                    return (int)(UtcEnd.Value - UtcStart).TotalMilliseconds;
                }

                return (int)(DateTimeOffset.UtcNow - UtcStart).TotalMilliseconds;
            }
        }
    }
}
