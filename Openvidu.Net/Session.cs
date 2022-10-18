using System;
using System.Collections.Generic;
using Openvidu.Net.Options;
using System.Text.Json.Serialization;
using Openvidu.Net.Core;
using Openvidu.Net.Enums;

namespace Openvidu.Net
{
    public class Session
    {
        public static class DefaultValues
        {
            public static MediaMode mediaMode = Enums.MediaMode.ROUTED;
            public static RecordingMode recordingMode = Enums.RecordingMode.MANUAL;
            public static RecordingProperties defaultRecordingProperties = new RecordingProperties();
            public static String customSessionId = "";
            public static String mediaNode = null;
            public static VideoCodec forcedVideoCodec = VideoCodec.VP8;
            public static Boolean allowTranscoding = false;
        }

    
        [JsonPropertyName("id")]
        public string Id { get; set; } 

        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("createdAt")]
        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("mediaMode")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MediaMode MediaMode { get; set; } = DefaultValues.mediaMode;

        [JsonPropertyName("recordingMode")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RecordingMode RecordingMode { get; set; } = DefaultValues.recordingMode;

        [JsonPropertyName("defaultRecordingProperties")]
        public RecordingProperties DefaultRecordingProperties { get; set; } = DefaultValues.defaultRecordingProperties;

        [JsonPropertyName("customSessionId")]
        public string CustomSessionId { get; set; } = DefaultValues.customSessionId;

        [JsonPropertyName("connections")]
        public Connections Connections { get; set; }

        [JsonPropertyName("recording")]
        public bool Recording { get; set; }

        [JsonPropertyName("forcedVideoCodec")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public VideoCodec ForcedVideoCodec { get; set; } = DefaultValues.forcedVideoCodec;

        [JsonPropertyName("allowTranscoding")]
        public bool AllowTranscoding { get; set; } = DefaultValues.allowTranscoding;

        [JsonPropertyName("mediaNodeId")]
        public string MediaNodeId { get; set; }
    }
    
}