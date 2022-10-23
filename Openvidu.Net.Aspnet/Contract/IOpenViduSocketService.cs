using Openvidu.Net.Aspnet.Event;

namespace Openvidu.Net.Aspnet.Contract;

public interface IOpenViduSocketService
{
    bool SendMessage(string sessionId, string message, params string[] members);

    bool SendMessage(OpenViduMessage message);

}