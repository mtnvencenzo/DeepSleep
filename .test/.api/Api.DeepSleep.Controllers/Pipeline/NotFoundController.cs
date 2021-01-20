namespace Api.DeepSleep.Controllers.Pipeline
{
    /// <summary>
    /// 
    /// </summary>
    public class NotFoundController
    {
        /// <summary>Gets this instance.</summary>
        /// <returns></returns>
        public NotFoundModel Get()
        {
            return new NotFoundModel();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class NotFoundModel
    {
        /// <summary>Gets a value indicating whether this <see cref="NotFoundModel"/> is found.</summary>
        /// <value><c>true</c> if found; otherwise, <c>false</c>.</value>
        public bool Found => true;
    }
}
