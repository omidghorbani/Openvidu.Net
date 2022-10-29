using Openvidu.Net.DTOs;
using System.Text.Json.Serialization;

namespace Openvidu.Net.Options
{
    public class MediaOptions
    {
        [JsonPropertyName("hasAudio")]
        public bool HasAudio { get; set; }

        [JsonPropertyName("audioActive")]
        public bool AudioActive { get; set; }

        [JsonPropertyName("hasVideo")]
        public bool HasVideo { get; set; }

        [JsonPropertyName("videoActive")]
        public bool VideoActive { get; set; }

        [JsonPropertyName("typeOfVideo")]
        public string TypeOfVideo { get; set; }

        [JsonPropertyName("frameRate")]
        public int FrameRate { get; set; }

        [JsonPropertyName("videoDimensions")]
        public string VideoDimensions { get; set; }

        //[JsonPropertyName("filter")]
        //public string[] Filter { get; set; }
    }
}