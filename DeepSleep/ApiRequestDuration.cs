namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestDuration
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestDuration"/> class.
        /// </summary>
        public ApiRequestDuration()
        {
            StartDate = DateTimeOffset.UtcNow;
            EndDate = DateTimeOffset.UtcNow;
        }

        #endregion

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        public DateTimeOffset EndDate { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>Gets or sets the duration.</summary>
        /// <value>The duration.</value>
        public int Duration
        {
            get
            {
                return (int)(EndDate - StartDate).TotalMilliseconds;
            }
        }
    }
}
