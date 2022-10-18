using Openvidu.Net.Enums;
using Openvidu.Net.Options;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Openvidu.Net.Core;

namespace Openvidu.Net
{

    public class Connections
    {
        public int numberOfElements { get; set; }
        public Connection[] content { get; set; }
    }
    public class Connection
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ConnectionType Type { get; set; }

        [JsonPropertyName("status")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ConnectionStatus Status { get; set; }

        [JsonPropertyName("sessionId")]
        public string SessionId { get; set; }

        [JsonPropertyName("createdAt")]
        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("activeAt")]
        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime ActiveAt { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("platform")]
        public string Platform { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("serverData")]
        public string ServerData { get; set; }

        [JsonPropertyName("clientData")]
        public string ClientData { get; set; }

        [JsonPropertyName("record")]
        public bool Record { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("kurentoOptions")]
        public KurentoOptions KurentoOptions { get; set; }

        [JsonPropertyName("rtspUri")]
        public string RtspUri { get; set; }

        [JsonPropertyName("adaptativeBitrate")]
        public bool? AdaptativeBitrate { get; set; }

        [JsonPropertyName("onlyPlayWithSubscribers")]
        public bool? OnlyPlayWithSubscribers { get; set; }

        [JsonPropertyName("networkCache")]
        public object NetworkCache { get; set; }

        [JsonPropertyName("publishers")]
        public List<Publisher> Publishers { get; set; }

        [JsonPropertyName("subscribers")]
        public List<Subscriber> Subscribers { get; set; }

        [JsonPropertyName("customIceServers")]
        public List<CustomIceServer> CustomIceServers { get; set; }
    }
}