using System.Threading.Tasks;

namespace DeepSleep.NetCore.Tests
{
    public class TestController
    {
        public Task Get()
        {
            return Task.CompletedTask;
        }
    }
}
