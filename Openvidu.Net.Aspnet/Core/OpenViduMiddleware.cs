using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Openvidu.Net.Aspnet.Contract;
using Openvidu.Net.Aspnet.Event;
using Openvidu.Net.Aspnet.Services;

namespace Openvidu.Net.Aspnet.Core;

public class OpenViduMiddleware
{
    private readonly IOpenViduService _openViduService;
    private readonly OpenViduWebhookOption _option;
    private readonly RequestDelegate _next;


    public OpenViduMiddleware(RequestDelegate next, IOpenViduService openViduService, OpenViduWebhookOption option)
    {
        _next = next;
        _openViduService = openViduService;
        _option = option;
    }


    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.Value == "/event/store" && context.Request.Method == "POST")
        {
            try
            {
                if (_option.AcceptHeaders.Any())
                {
                    foreach (var header in _option.AcceptHeaders)
                    {
                        if (context.Request.Headers.Any(a => a.Key == header.key))
                        {
                            
                            if (context.Request.Headers[header.key].ToString() != header.value)
                                throw new Exception($"the value of header[{header.key}] not valid!");
                        }
                        else
                        {
                            throw new Exception($"the header {header.key} not found!");
                        }

                    }

                }
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