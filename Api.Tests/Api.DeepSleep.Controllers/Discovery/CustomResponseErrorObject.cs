using System.Collections.Generic;

namespace Api.DeepSleep.Controllers.Discovery
{
    public class CustomResponseErrorObject
    {
        public CustomResponseErrorObject(IList<string> errors)
        {
            this.Errors = errors;
        }

        public string Test => "Value";

        public IList<string> Errors { get; set; }
    }
}
