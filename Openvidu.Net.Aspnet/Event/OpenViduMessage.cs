namespace Openvidu.Net.Aspnet.Event;

public class OpenViduMessage
{
    public string Message { get; set; }
    public string SessionId { get; set; }
    public string[] Members { get; set; }
}