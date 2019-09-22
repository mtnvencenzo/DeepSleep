using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="resolver">The resolver.</param>
    /// <returns></returns>
    public delegate Task ApiRequestDelegate(IApiRequestContextResolver resolver);
}
