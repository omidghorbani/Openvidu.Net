using System.Text.Json.Serialization;

namespace Openvidu.Net.DTOs;

public class MediaNodeAws
{

    /// <summary>
    /// instanceType (optional String only for AWS deployments) : a valid EC2 instance type. If specified, a Media Node will be created using the specified EC2 instance type. If not especified, value AWS_INSTANCE_TYPE in /opt/openvidu/.env will be used. This property is only taken into account in AWS deployments.
    /// </summary>
    [JsonPropertyName("instanceType")]
    public string InstanceType { get; set; }

    /// <summary>
    /// volumeSize (optional Number only for AWS deployments) : Volume size for the new Media Node in GB. If specified, the Media Node will be created with such disk volume size. If not especified, value AWS_VOLUME_SIZE in /opt/openvidu/.env will be used. This property is only taken into account in AWS deployments.
    /// </summary>
    [JsonPropertyName("volumeSize")]
    public int VolumeSize { get; set; }
}