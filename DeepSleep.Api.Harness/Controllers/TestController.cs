using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeepSleep.Api.Harness.Controllers
{
    public class TestController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public TestRs Post([BodyBound]TestRq request)
        {
            return new TestRs
            {
                Value = request.Value
            };
        }
    }
}
