using Newtonsoft.Json;
using Reflux.Model;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using static System.String;

namespace Reflux.Services
{
    public class SearchService
    {
        private readonly string _apiKey;
        private readonly UserService _userService;
        private readonly FlowService _flowService;

        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public SearchService(string apiKey)
        {
            if (IsNullOrEmpty(apiKey))
            {
                _apiKey = ConfigurationManager.AppSettings["FlowdockAPIKey"];
            }
            else
            {
                _apiKey = apiKey;
            }

            _userService = new UserService(_apiKey);
            _flowService = new FlowService(_apiKey);

            _users = _userService.GetUsers();
            _flows = _flowService.GetFlows();

            logger.Info("Found " + Users.Count + " users.");
            logger.Info("Found " + Flows.Count + " flows to search.");
        }

        private List<User> _users;
        public List<User> Users => _users ?? (_users = _userService.GetUsers());

        private List<Flow> _flows;
        public List<Flow> Flows => _flows ?? (_flows = _flowService.GetFlows());

        public List<MessageSearchResult> Search(string searchtext)
        {
            var result = new List<MessageSearchResult>();

            foreach (var flow in Flows)
            {
                var url = $"{Constants.ApiUrl}{"flows/"}{Constants.CompanyName}{"/"}{flow.ParameterizedName}{"/messages/?event=message&search="}{searchtext}";

                var responseText = Internet.Get(url, _apiKey);

                var searchResults = JsonConvert.DeserializeObject<List<MessageSearchResult>>(responseText)
                    .Select(r => {
                        r.OriginalFlowName = flow.ParameterizedName;
                        r.FlowName = flow.Name;
                        r.UserName = Users.FirstOrDefault(u => u.Id == r.UserId)?.Name;
                        return r;
                    });

                result.AddRange(searchResults);
            }

            return result;
        }
    }
}
