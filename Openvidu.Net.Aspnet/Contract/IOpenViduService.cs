using Openvidu.Net.Aspnet.Event;

namespace Openvidu.Net.Aspnet.Contract;

public interface IOpenViduService
{
    void StoreEvent(OpenViduEvent @event);
    void RegisterHandler(Func<OpenViduEvent, Task>? func);
}