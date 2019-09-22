using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeepSleep.Example.Controllers.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class HelloWorldPostBodyRq : IValidatableObject
    {
        /// <summary>Gets or sets the message.</summary>
        /// <value>The message.</value>
        [StringLength(10, ErrorMessage = "400.11111|Message is invalid")]
        public string Message { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (Message == null)
            {
                results.Add(new ValidationResult("400.22222|Message is required"));
            }

            return results;
        }
    }
}
