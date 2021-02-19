namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    internal static class NumericExtensions
    {
        /// <summary>Determines whether the specified lower is between.</summary>
        /// <param name="inVal">The in value.</param>
        /// <param name="lower">The lower.</param>
        /// <param name="upper">The upper.</param>
        /// <returns>
        ///   <c>true</c> if the specified lower is between; otherwise, <c>false</c>.
        /// </returns>
        internal static bool IsBetween(this int? inVal, int lower, int upper)
        {
            return ((inVal ?? 0) >= lower && (inVal ?? 0) <= upper);
        }

        /// <summary>Determines whether the specified lower is between.</summary>
        /// <param name="inVal">The in value.</param>
        /// <param name="lower">The lower.</param>
        /// <param name="upper">The upper.</param>
        /// <returns><c>true</c> if the specified lower is between; otherwise, <c>false</c>.</returns>
        internal static bool IsBetween(this int inVal, int lower, int upper)
        {
            return inVal >= lower && inVal <= upper;
        }
    }
}
