using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Openvidu.Net.DTOs
{
    public class MediaNode
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("environmentId")]
        public string EnvironmentId { get; set; }

        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonPropertyName("launchingTime")]
        public long LaunchingTime { get; set; }

        [JsonPropertyName("connected")]
        public bool Connected { get; set; }

        [JsonPropertyName("connectionTime")]
        public long ConnectionTime { get; set; }

        [JsonPropertyName("load")]
        public double Load { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("sessions")]
        public List<Session> Sessions { get; set; }

        [JsonPropertyName("recordingIds")]
        public List<Recording> RecordingIds { get; set; }

        [JsonPropertyName("kurentoInfo")]
        public KurentoInfo KurentoInfo { get; set; }
    }
}