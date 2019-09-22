using System;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ApiRequestDuration
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestDuration"/> class.
        /// </summary>
        public ApiRequestDuration()
        {
            ServerTime = DateTime.UtcNow;
            EndDate = DateTime.MaxValue.ChangeKind(DateTimeKind.Utc);
        }

        #endregion

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public DateTime ServerTime { get; set; }

        /// <summary>Gets or sets the duration.</summary>
        /// <value>The duration.</value>
        public int Duration
        {
            get
            {
                return (int)(EndDate - ServerTime).TotalMilliseconds;
            }
        }
    }
}
