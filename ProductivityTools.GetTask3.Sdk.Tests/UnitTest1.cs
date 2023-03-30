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
            var builder = new ConfigurationBuilder().AddJsonFile("appSettings.json", optional: false);
            var configuration = builder.Build();
            var password = configuration["password"];
            var webapikey = configuration["WebApiKey"];
            //var url = "https://apigettask3.productivitytools.top:8040/api/";
            var url = "http://localhost:5513/api/";
            var taskClient = new TaskClient(url, webapikey, (x) => { System.Console.WriteLine(x); });
            //this is not working as we do not have user in request now
            //var result = taskClient.GetStructure(null, string.Empty).Result;

            var result2 = taskClient.GetThisWeekFinishedForUser(null, string.Empty,"pwujczyk@gmail.com").Result;
        }
    }
}