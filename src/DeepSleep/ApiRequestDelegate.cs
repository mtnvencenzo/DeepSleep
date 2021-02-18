namespace DeepSleep
{
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="resolver">The resolver.</param>
    /// <returns></returns>
    public delegate Task ApiRequestDelegate(IApiRequestContextResolver resolver);
}
