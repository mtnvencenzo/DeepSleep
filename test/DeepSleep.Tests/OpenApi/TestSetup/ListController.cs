namespace DeepSleep.Tests.OpenApi.TestSetup
{
    using DeepSleep.OpenApi.Decorators;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ListController
    {
        public Task<IList<ListObj>> List()
        {
            throw new NotImplementedException();
        }

        public IList<ListObj1> List1()
        {
            throw new NotImplementedException();
        }

        [OasResponse("200", typeof(IList<ListObj2>))]
        public Task<IList<ListObj2>> List2()
        {
            throw new NotImplementedException();
        }

        [OasResponse("200", typeof(ListResponseContainer))]
        public Task<ListResponseContainer> ListContainer()
        {
            throw new NotImplementedException();
        }

        //
    }
}
