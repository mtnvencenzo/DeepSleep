namespace DeepSleep.JsonPatch
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonPatchOperationError
    {
        #region Constructors & Initialization

        /// <summary>Initializes a new instance of the <see cref="JsonPatchOperationError"/> class.</summary>
        /// <param name="operation">The operation.</param>
        /// <param name="type">The type.</param>
        public JsonPatchOperationError(JsonPatchOperation operation, JsonPatchOperationErrorType type)
        {
            Operation = operation;
            Type = type;
        }

        #endregion

        /// <summary>Gets or sets the operation.</summary>
        /// <value>The operation.</value>
        public JsonPatchOperation Operation { get; private set; }

        /// <summary>Gets the type.</summary>
        /// <value>The type.</value>
        public JsonPatchOperationErrorType Type { get; private set; }
    }
}
