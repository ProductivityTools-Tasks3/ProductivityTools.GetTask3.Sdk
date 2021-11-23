using ProductivityTools.GetTask3.Contract;
using ProductivityTools.GetTask3.Contract.Requests;
using ProductivityTools.GetTask3.Sdk;
using System;
using System.Threading.Tasks;

namespace ProductivityTools.GetTask3.Sdk
{
    public class TaskClient
    {

        public async static Task<object> Start(int elementId, Action<string> log)
        {
            return await GetTaskHttpClient.Post2<object>(Consts.Task, Consts.Start, new StartRequest() { ElementId = elementId }, log);
        }

        public async static Task<ElementView> GetStructure(int? currentNode, string path, Action<string> log)
        {
            var rootElement = await GetTaskHttpClient.Post2<ElementView>(Consts.Task, Consts.TodayList, new ListRequest() { ElementId = currentNode, Path = path },log);
            return rootElement;
        }
    }
}
