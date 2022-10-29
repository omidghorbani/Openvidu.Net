using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Openvidu.Net.Aspnet.Contract;
using Openvidu.Net.Aspnet.Services;

namespace Openvidu.Net.Aspnet.Core
{
    public static class OpenViduRouterExtensions
    {
        public static IServiceCollection ConfigureOpenVidu(this IServiceCollection services)
        {
            services.AddSingleton<IOpenViduService, OpenViduService>();
            //services.AddHostedService<OpenViduService>();

            return services;
        }

        public static IApplicationBuilder UseOpenVidu(this IApplicationBuilder app)
        {
            app.UseMiddleware<OpenViduMiddleware>();

            return app;
        }
    }
}