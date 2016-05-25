using Newtonsoft.Json;
using System;

namespace Reflux.Model
{
    public class Flow
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parameterized_name")]
        public string ParameterizedName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("access_mode")]
        public string AccessMode { get; set; }

        [JsonProperty("flow_admin")]
        public bool IsAdmin { get; set; }

        [JsonProperty("api_token")]
        public string ApiToken { get; set; }

        [JsonProperty("last_message_at")]
        public DateTime? LastMessageAt { get; set; }
    }
}
