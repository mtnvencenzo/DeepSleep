namespace Api.DeepSleep.Controllers.Formatters
{
    using Api.DeepSleep.Controllers.Binding;

    /// <summary>
    /// 
    /// </summary>
    public class AcceptController
    {
        /// <summary>Gets the XML only.</summary>
        /// <returns></returns>
        public SimpleUrlBindingRs Get()
        {
            return new SimpleUrlBindingRs
            {
                BoolVar = true
            };
        }
    }
}
