using System.Net.WebSockets;
using Microsoft.Extensions.Hosting;
using Openvidu.Net.Aspnet.Contract;
using Openvidu.Net.Aspnet.Core;
using Openvidu.Net.Aspnet.Event;

namespace Openvidu.Net.Aspnet.Services;

public class OpenViduSocketService : IOpenViduSocketService,  IDisposable
{
    private CancellationTokenSource cts;
    private static Dictionary<string, ClientWebSocket> _clients = new();
    private IOpenViduService _openViduService;
    public OpenViduSocketService(CancellationToken cts, IOpenViduService openViduService)
    {
        _openViduService = openViduService;
        this.cts = new CancellationTokenSource();
    }


    public bool SendMessage(string sessionId, string message, params string[] members)
    {
        return SendMessage(new OpenViduMessage()
        {
            SessionId = sessionId,
            Message = message,
            Members = members
        });
    }

    public bool SendMessage(OpenViduMessage message)
    {
        throw new NotImplementedException();
    }

    public Func<OpenViduMessage, Task> Connect(string sessionId, Func<OpenViduMessage, Task> func)
    {
        throw new NotImplementedException();
    }


    public void Dispose()
    {
    }
}