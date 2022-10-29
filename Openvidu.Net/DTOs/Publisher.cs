using Openvidu.Net.Options;
using System;
using System.Text.Json.Serialization;
using Openvidu.Net.Core;

namespace Openvidu.Net.DTOs
{
    public class Publisher
    {
        [JsonPropertyName("createdAt")]
        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("streamId")]
        public string StreamId { get; set; }

        [JsonPropertyName("mediaOptions")]
        public MediaOptions MediaOptions { get; set; }
    }
}