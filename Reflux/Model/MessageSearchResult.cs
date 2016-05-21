using Newtonsoft.Json;

namespace Reflux.Model
{
    public class MessageSearchResult : SearchResult
    {
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
