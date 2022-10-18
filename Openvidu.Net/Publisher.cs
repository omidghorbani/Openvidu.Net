using Openvidu.Net.Options;
using System;
using System.Text.Json.Serialization;

namespace Openvidu.Net
{
    public class Publisher
    {
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("streamId")]
        public string StreamId { get; set; }

        [JsonPropertyName("mediaOptions")]
        public MediaOptions MediaOptions { get; set; }
    }
}