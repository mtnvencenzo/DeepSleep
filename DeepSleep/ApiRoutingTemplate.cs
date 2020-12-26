namespace DeepSleep
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("{Template}")]
    public class ApiRoutingTemplate
    {
        /// <summary>Initializes a new instance of the <see cref="ApiRoutingTemplate"/> class.</summary>
        /// <param name="template">The template.</param>
        public ApiRoutingTemplate(string template)
        {
            Template = template;
            Variables = GetTemplateVariables(template);
        }

        /// <summary>Gets or sets the template.</summary>
        /// <value>The template.</value>
        public string Template { get; private set; }

        /// <summary>Gets or sets the locations.</summary>
        /// <value>The locations.</value>
        public IList<ApiEndpointLocation> Locations { get; } = new List<ApiEndpointLocation>();

        /// <summary>Gets or sets the variables.</summary>
        /// <value>The variables.</value>
        public IList<string> Variables { get; private set; }

        /// <summary>Gets the template variables.</summary>
        /// <param name="template">The template.</param>
        /// <returns></returns>
        protected virtual IList<string> GetTemplateVariables(string template)
        {
            if (string.IsNullOrWhiteSpace(template))
            {
                return new List<string>();
            }

            var variables = new List<string>();
            var parts = template.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                if (part.StartsWith("{") && part.EndsWith("}"))
                {
                    var trimmed = part
                        .Replace("{", string.Empty)
                        .Replace("}", string.Empty);

                    if (trimmed.Length == (part.Length - 2))
                    {
                        var variable = part
                            .Substring(1, part.Length - 2)
                            .Replace(" ", string.Empty)
                            .Trim();

                        if (!string.IsNullOrWhiteSpace(variable))
                        {
                            variables.Add(variable);
                        }
                    }
                }
            }

            return variables;
        }
    }
}
