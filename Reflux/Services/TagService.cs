using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Reflux.Model;

namespace Reflux.Services
{
    public class TagService
    {
        private readonly string _apiKey;

        public TagService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public void AddTag(string messageId, string flowName, string message)
        {
            var url = $"{Constants.BaseUrl}{"flows/"}{Constants.CompanyName}{"/"}{flowName}{"/messages/"}{messageId}";

            Internet.Put(url, _apiKey, "{ \"tags\":  [\"" + message + "\"] }");
        }
    }
}
