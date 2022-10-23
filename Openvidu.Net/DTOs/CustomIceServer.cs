using System.Text.Json.Serialization;

namespace Openvidu.Net.DTOs
{
    public class CustomIceServer
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("credential")]
        public string Credential { get; set; }
    }
}
