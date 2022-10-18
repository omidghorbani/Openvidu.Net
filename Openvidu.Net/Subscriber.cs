using System;
using System.Text.Json.Serialization;

namespace Openvidu.Net
{
    public class Subscriber
    {
        [JsonPropertyName("streamId")]
        public string StreamId { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}