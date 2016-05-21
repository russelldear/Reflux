using Newtonsoft.Json;
using System;

namespace Reflux.Model
{
    public class SearchResult
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("user")]
        public string UserId { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("thread_id")]
        public string ThreadId { get; set; }

        [JsonProperty("flow")]
        public string FlowId { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("edited_at")]
        public DateTime? EditedAt { get; set; }

        public string CreatedAtString => CreatedAt.ToString("yyyy-MM-dd hh:mm:ss");

        public string OriginalFlowName { get; set; }

        public string FlowName { get; set; }

        public string UserName { get; set; }

        public string Url => $"https://www.flowdock.com/app/{Constants.CompanyName}/{OriginalFlowName}/threads/{ThreadId}";
    }
}
