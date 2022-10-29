using System;
using System.Text.Json.Serialization;
using Openvidu.Net.Core;
using Openvidu.Net.Enums;

namespace Openvidu.Net.DTOs
{
    public class Recording
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("outputMode")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RecordingOutputMode OutputMode { get; set; }

        [JsonPropertyName("hasAudio")]
        public bool HasAudio { get; set; }

        [JsonPropertyName("hasVideo")]
        public bool HasVideo { get; set; }

        [JsonPropertyName("resolution")]
        public string Resolution { get; set; }

        [JsonPropertyName("frameRate")]
        public int FrameRate { get; set; }

        [JsonPropertyName("recordingLayout")]
        public string RecordingLayout { get; set; }

        [JsonPropertyName("customLayout")]
        public string CustomLayout { get; set; }

        [JsonPropertyName("sessionId")]
        public string SessionId { get; set; }

        [JsonPropertyName("createdAt")]
        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("duration")]
        public decimal Duration { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}