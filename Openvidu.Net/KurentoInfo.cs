using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Openvidu.Net
{
    public class KurentoInfo
    {
        [JsonPropertyName("numberOfElements")]
        public int NumberOfElements { get; set; }

        [JsonPropertyName("content")]
        public List<MediaNode> Content { get; set; }
    }
}