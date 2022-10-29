using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Openvidu.Net.Enums;

namespace Openvidu.Net.DTOs
{
    public class RecordingProperties
    {


        public RecordingProperties(string name = "")
        {

            HasAudio = true;
            HasVideo = true;
            OutputMode = RecordingOutputMode.COMPOSED;
            RecordingLayout = "BEST_FIT";
            Resolution = "1280x720";
            FrameRate = 25;
            ShmSize = 536870912;

            Name = string.IsNullOrEmpty(name) ? new Random().Next(1000, 9999).ToString() : name;

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