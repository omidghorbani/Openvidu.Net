﻿using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Openvidu.Net.Enums;

namespace Openvidu.Net
{
    public class RecordingProperties
    {
        public static class DefaultValues
        {
            public static String name = "";
            public static Boolean hasAudio = true;
            public static Boolean hasVideo = true;
            public static RecordingOutputMode outputMode = RecordingOutputMode.COMPOSED;
            public static RecordingLayout recordingLayout = Enums.RecordingLayout.BEST_FIT;
            public static String resolution = "1280x720";
            public static int frameRate = 25;
            public static long shmSize = 536870912L;
            public static String customLayout = "";
            public static Boolean ignoreFailedStreams = false;
        }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("hasAudio")]
        public bool HasAudio { get; set; }

        [JsonPropertyName("hasVideo")]
        public bool HasVideo { get; set; }

        [JsonPropertyName("outputMode")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RecordingOutputMode OutputMode { get; set; }

        [JsonPropertyName("recordingLayout")]
        public string RecordingLayout { get; set; }

        [JsonPropertyName("resolution")]
        public string Resolution { get; set; }

        [JsonPropertyName("frameRate")]
        public int FrameRate { get; set; }

        [JsonPropertyName("shmSize")]
        public int ShmSize { get; set; }

        [JsonPropertyName("mediaNode")]
        public MediaNode MediaNode { get; set; }


        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}