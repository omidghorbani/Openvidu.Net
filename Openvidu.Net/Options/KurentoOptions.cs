using Openvidu.Net.DTOs;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Openvidu.Net.Options
{
    public class KurentoOptions
    {
        [JsonPropertyName("videoMaxRecvBandwidth")]
        public int VideoMaxRecvBandwidth { get; set; }

        [JsonPropertyName("videoMinRecvBandwidth")]
        public int VideoMinRecvBandwidth { get; set; }

        [JsonPropertyName("videoMaxSendBandwidth")]
        public int VideoMaxSendBandwidth { get; set; }

        [JsonPropertyName("videoMinSendBandwidth")]
        public int VideoMinSendBandwidth { get; set; }

        [JsonPropertyName("allowedFilters")] public List<Filter> AllowedFilters { get; set; }
    }
}