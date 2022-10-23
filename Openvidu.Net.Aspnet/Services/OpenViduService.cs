using System.Threading.Channels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Openvidu.Net.Aspnet.Contract;
using Openvidu.Net.Aspnet.Event;

namespace Openvidu.Net.Aspnet.Services;

public class OpenViduService : IOpenViduService, IHostedService
{
    private readonly Channel<OpenViduEvent> _jobs;
    private readonly ILogger<OpenViduService> _logger;

    private Func<OpenViduEvent, Task>? _func;
    
    public OpenViduService(IHostApplicationLifetime applicationLifetime, ILogger<OpenViduService> logger)
    {
        _logger = logger;
        _jobs = Channel.CreateUnbounded<OpenViduEvent>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false
        });

        applicationLifetime.ApplicationStopping.Register(() => _jobs?.Writer.TryComplete());
        applicationLifetime.ApplicationStarted.Register(async () => await ProcessQueue());
    }

    public void RegisterHandler(Func<OpenViduEvent, Task>? func)
    {
        _func = func;
    }

    public void StoreEvent(OpenViduEvent @event)
    {
        _jobs.Writer.TryWrite(@event);
    }

    private async Task ProcessQueue()
    {
        while (!_jobs.Reader.Completion.IsCompleted)
        {
            try
            {
                if (!await _jobs.Reader.WaitToReadAsync())
                    break;
                if (_func == null)
                    break;

                OpenViduEvent? item;
                while (_jobs.Reader.TryRead(out item))
                {
                    if (_func != null)
                    {
                        await _func(item);

                    }
                }

            }
            catch (ChannelClosedException)
            {
                break;
            }
            catch (Exception ex)
            {
                break;

            }
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        
        _logger.LogInformation($"The {nameof(OpenViduService)} started.");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        
        _logger.LogInformation($"The {nameof(OpenViduService)} stopping.");

        _jobs.Writer.TryComplete();

        _logger.LogInformation($"The {nameof(OpenViduService)} stopped.");

        return Task.CompletedTask;

    }
}