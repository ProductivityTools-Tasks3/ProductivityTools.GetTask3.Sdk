using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProductivityTools.GetTask3.Sdk.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appSettings.json", optional:false);
            var configuration = builder.Build();
            var password = configuration["password"];
            var taskClient = new TaskClient("http://localhost:5513/api/",password, (x) => { System.Console.WriteLine(x); });
            var result = taskClient.GetStructure(null, string.Empty).Result;
        }
    }
}