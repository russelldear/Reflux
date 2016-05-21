using Newtonsoft.Json;
using Reflux.Model;
using System.Collections.Generic;

namespace Reflux.Services
{
    public class FlowService
    {
        private readonly string _apiKey;

        public FlowService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public List<Flow> GetFlows()
        {
            var url = $"{Constants.BaseUrl}{"flows"}";

            var responseText = Internet.Get(url, _apiKey);

            return JsonConvert.DeserializeObject<List<Flow>>(responseText);
        }
    }
}
