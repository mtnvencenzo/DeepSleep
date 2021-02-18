namespace DeepSleep.Tests.TestArtifacts
{
    public class NonDefaultConstructorModel
    {
        public NonDefaultConstructorModel(string mystring)
        {
            MyString = mystring;
        }

        public string MyString { get; set; }
    }
}
