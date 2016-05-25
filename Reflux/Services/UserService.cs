using Newtonsoft.Json;
using Reflux.Model;
using System.Collections.Generic;

namespace Reflux.Services
{
    public class UserService
    {
        private readonly string _apiKey;

        public UserService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public List<User> GetUsers()
        {
            var url = $"{Constants.ApiUrl}{"users"}";

            var responseText = Internet.Get(url, _apiKey);

            return JsonConvert.DeserializeObject<List<User>>(responseText);
        }
    }
}
