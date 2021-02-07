namespace DeepSleep.OpenApi.Decorators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(validOn: AttributeTargets.Method, AllowMultiple = false)]
    public class OasApiOperationAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="OasApiOperationAttribute" /> class.</summary>
        /// <param name="operationId">The operation identifier.</param>
        /// <param name="tags">The tags.</param>
        public OasApiOperationAttribute(string operationId, string[] tags = null)
        {
            this.OperationId = operationId ?? string.Empty;

            this.Tags = tags != null
                ? new List<string>(tags.Where(t => t != null))
                : new List<string>();
        }

        /// <summary>Gets the operation identifier.</summary>
        /// <value>The operation identifier.</value>
        public string OperationId { get; }

        /// <summary>Gets the tags.</summary>
        /// <value>The tags.</value>
        public IList<string> Tags { get; }
    }
}
