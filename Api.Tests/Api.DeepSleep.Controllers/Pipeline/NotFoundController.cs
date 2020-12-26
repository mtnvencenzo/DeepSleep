namespace Api.DeepSleep.Controllers.Pipeline
{
    public class NotFoundController
    {
        public NotFoundModel Get()
        {
            return new NotFoundModel();
        }
    }

    public class NotFoundModel
    {
        public bool Found => true;
    }
}
