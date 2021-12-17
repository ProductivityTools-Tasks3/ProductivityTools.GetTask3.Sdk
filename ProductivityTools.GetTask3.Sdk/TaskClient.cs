using ProductivityTools.GetTask3.Contract;
using ProductivityTools.GetTask3.Contract.Requests;
using ProductivityTools.GetTask3.Sdk;
using System;
using System.Threading.Tasks;

namespace ProductivityTools.GetTask3.Sdk
{
    public class TaskClient
    {
        private readonly string Address;
        private readonly GetTaskHttpClient GetTaskHttpClient;

        public TaskClient(string address)
        {
            this.Address = address;
            GetTaskHttpClient = new GetTaskHttpClient(this.Address);
        }

        public async Task<object> Start(int elementId, Action<string> log)
        {
            return await GetTaskHttpClient.Post2<object>(Consts.Task, Consts.Start, new StartRequest() { ElementId = elementId }, log);
        }

        public async Task<ElementView> GetStructure(int? currentNode, string path, Action<string> log)
        {
            var rootElement = await GetTaskHttpClient.Post2<ElementView>(Consts.Task, Consts.TodayList, new ListRequest() { ElementId = currentNode, Path = path },log);
            return rootElement;
        }
    }
}
