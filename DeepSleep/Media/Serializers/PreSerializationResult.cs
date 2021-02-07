namespace DeepSleep.Media.Serializers
{
    /// <summary>
    /// 
    /// </summary>
    public class PreSerializationResult
    {
        private PreSerializationResult()
        {
        }

        /// <summary>Gets the object result.</summary>
        /// <value>The object result.</value>
        public object ObjectResult { get; private set; }

        /// <summary>Gets a value indicating whether this instance has read.</summary>
        /// <value><c>true</c> if this instance has read; otherwise, <c>false</c>.</value>
        public bool HasRead { get; private set; }

        /// <summary>Handleds the specified object result.</summary>
        /// <param name="objectResult">The object result.</param>
        /// <returns></returns>
        public static PreSerializationResult Handled(object objectResult)
        {
            return new PreSerializationResult
            {
                ObjectResult = objectResult,
                HasRead = true
            };
        }

        /// <summary>Nots the handled.</summary>
        /// <returns></returns>
        public static PreSerializationResult NotHandled()
        {
            return new PreSerializationResult
            {
                ObjectResult = null,
                HasRead = false
            };
        }
    }
}
