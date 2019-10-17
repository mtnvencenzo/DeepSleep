namespace DeepSleep.JsonPatch
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonPatchOperationOutcome
    {
        /// <summary>Gets or sets a value indicating whether this <see cref="JsonPatchOperationOutcome"/> is success.</summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success { get; set; }

        /// <summary>Gets or sets the error.</summary>
        /// <value>The error.</value>
        public JsonPatchOperationError Error { get; set; }
    }
}
