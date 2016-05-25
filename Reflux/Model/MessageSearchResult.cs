using Newtonsoft.Json;
using System.Collections.Generic;

namespace Reflux.Model
{
    public class MessageSearchResult : SearchResult
    {
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }
    }
}
