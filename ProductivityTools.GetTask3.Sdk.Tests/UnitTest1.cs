using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ProductivityTools.GetTask3.Sdk.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"D:\GitHub\Home.Configuration\ptprojectsweb-firebase-adminsdk.json");
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false);
            var configuration = builder.Build();
            var webapikey = configuration["WebApiKey"];
            var url = "https://tasks-api.productivitytools.top/api/";
            //var url = "http://localhost:5513/api/";
            
          var taskClient = new TaskClient(url, webapikey, (x) => { System.Console.WriteLine(x); });
            
            //this is not working as we do not have user in request now
            //var result = taskClient.GetStructure(null, string.Empty).Result;

            var result2 = taskClient.GetThisWeekFinishedForUser(null, string.Empty,"pwujczyk@gmail.com").Result;
        }
    }
}