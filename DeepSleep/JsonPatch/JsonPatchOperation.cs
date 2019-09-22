using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeepSleep.JsonPatch
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonPatchOperation
    {
        /// <summary>
        /// Gets or sets the op.
        /// </summary>
        /// <value>
        /// The op.
        /// </value>
        public string op { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string path { get; set; }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        public string from { get; set; }

        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        public string value { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class JsonPatchOperationExtensionMethods
    {
        #region Private Methos

        #endregion

        /// <summary>Applies to.</summary>
        /// <param name="operations">The operations.</param>
        /// <param name="to">To.</param>
        public static void ApplyTo(this IEnumerable<JsonPatchOperation> operations, object to)
        {
            operations.ApplyTo(to, null);
        }

        /// <summary>Applies to.</summary>
        /// <param name="operations">The operations.</param>
        /// <param name="to">To.</param>
        /// <param name="errorHandler">The error handler.</param>
        public static void ApplyTo(this IEnumerable<JsonPatchOperation> operations, object to, Action<JsonPatchOperationError> errorHandler)
        {
            if (operations == null || operations.Count() == 0 || to == null)
                return;

            var toType = to.GetType();

            if (toType == typeof(string) || toType.IsArray || typeof(System.Collections.IEnumerable).IsAssignableFrom(toType))
                return;

            var ops = operations.Where(o => o != null).ToList();

            foreach (var operation in ops)
            {
                var outcome = operation.ApplyTo(to, toType, errorHandler);

                if (outcome.Success == false)
                {
                    break;
                }
            }
        }

        /// <summary>Applies to.</summary>
        /// <param name="operation">The operation.</param>
        /// <param name="to">To.</param>
        /// <param name="toType">To type.</param>
        /// <param name="errorHandler">The error handler.</param>
        /// <returns></returns>
        private static JsonPatchOperationOutcome ApplyTo(this JsonPatchOperation operation, object to, Type toType, Action<JsonPatchOperationError> errorHandler)
        {
            if (!IsValidOperationType(operation.op))
                return AssertError(operation, errorHandler, new JsonPatchOperationError(operation, JsonPatchOperationErrorType.InvalidOp));

            //if(operation.op == "add" && !IsValidAddOperation(operation, out var errorType))
            //    return AssertError(operation, errorHandler, new JsonPatchOperationError(operation, errorType));


            return new JsonPatchOperationOutcome
            {
                Success = true
            };
        }

        /// <summary>
        /// Determines whether [is valid operation type] [the specified op].
        /// </summary>
        /// <param name="op">The op.</param>
        /// <returns><c>true</c> if [is valid operation type] [the specified op]; otherwise, <c>false</c>.</returns>
        private static bool IsValidOperationType(string op)
        {
            if (op == "replace")
                return true;
            if (op == "add")
                return true;
            if (op == "remove")
                return true;
            if (op == "move")
                return true;
            if (op == "copy")
                return true;
            if(op == "test")
                return true;

            return false;
        }

        //private static bool IsValidAddOperation(JObject document, string[] path, string value, out JsonPatchOperationErrorType errorType)
        //{
        //    if(document[
        //}

        /// <summary>Asserts the error.</summary>
        /// <param name="operation">The operation.</param>
        /// <param name="errorHandler">The error handler.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        /// <exception cref="DeepSleep.JsonPatch.JsonPatchOperationException"></exception>
        private static JsonPatchOperationOutcome AssertError(JsonPatchOperation operation, Action<JsonPatchOperationError> errorHandler, JsonPatchOperationError error)
        {
            if (errorHandler == null)
            {
                throw new JsonPatchOperationException(error);
            }
            else
            {
                errorHandler(error);

                return new JsonPatchOperationOutcome
                {
                    Error = error,
                    Success = false
                };
            }
        }
    }
}
