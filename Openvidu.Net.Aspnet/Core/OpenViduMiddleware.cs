using System.Text;
using Microsoft.AspNetCore.Http;
using Openvidu.Net.Aspnet.Event;
using Openvidu.Net.Aspnet.Services;

namespace Openvidu.Net.Aspnet.Core;

public class OpenViduMiddleware : IMiddleware
{
    private readonly OpenViduService _openViduService;

    public OpenViduMiddleware(OpenViduService openViduService)
    {
        _openViduService = openViduService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path.Value == "/event/store")
        {
            var req = context.Request;

            using StreamReader reader = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true);

            var bodyStr = await reader.ReadToEndAsync();

            _openViduService.StoreEvent(new OpenViduEvent() { Json = bodyStr });

            await context.Response.WriteAsync("OK");

        }

        await next(context);
    }
}