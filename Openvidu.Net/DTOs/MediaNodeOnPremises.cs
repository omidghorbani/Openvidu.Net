using System.Text.Json.Serialization;

namespace Openvidu.Net.DTOs;

public class MediaNodeOnPremises
{
    /// <summary>
    /// uri (mandatory String only for On Premises deployments) : the websocket endpoint of a running Media Node. Should be something similar to ws://media.server.ip:8888/kurento. This property is only necessary and is only taken into account On Premises deployments. For other deployment environments a new Media Node will be automatically launched completely ignoring parameter uri.
    /// </summary>
    [JsonPropertyName("uri")]
    public string Uri { get; set; }

    /// <summary>
    /// environmentId (optional String only for On Premises deployments) : a custom environment id. This can help further identify your on premises Media Node.
    /// </summary>
    [JsonPropertyName("environmentId")]
    public int EnvironmentId { get; set; }
}