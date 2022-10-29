using System.Text;
using Microsoft.AspNetCore.Http;
using Openvidu.Net.Aspnet.Contract;
using Openvidu.Net.Aspnet.Event;
using Openvidu.Net.Aspnet.Services;

namespace Openvidu.Net.Aspnet.Core;

public class OpenViduMiddleware
{
    private readonly IOpenViduService _openViduService;
    private readonly RequestDelegate _next;

    public OpenViduMiddleware(RequestDelegate next, IOpenViduService openViduService)
    {
        _next = next;
        _openViduService = openViduService;
    }


    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.Value == "/event/store" && context.Request.Method == "POST")
        {
            try
            {
                var req = context.Request;

                using StreamReader reader = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true);

                var bodyStr = await reader.ReadToEndAsync();
                
                _openViduService.StoreEvent(new OpenViduEvent() { Json = bodyStr });
                
            }
            catch (Exception ex)
            {

                
            }

        }

        await _next(context);
    }
}