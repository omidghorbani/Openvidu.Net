using System.Threading.Channels;
using Microsoft.Extensions.Hosting;
using Openvidu.Net.Aspnet.Contract;
using Openvidu.Net.Aspnet.Event;

namespace Openvidu.Net.Aspnet.Services;

public class OpenViduService : IOpenViduService
{
    private Channel<OpenViduEvent> jobs = null;

    private Func<OpenViduEvent, Task> _func = null;

    public OpenViduService(IHostApplicationLifetime applicationLifetime)
    {
        jobs = Channel.CreateUnbounded<OpenViduEvent>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false
        });

        applicationLifetime.ApplicationStopping.Register(() => jobs?.Writer.TryComplete());
        applicationLifetime.ApplicationStarted.Register(async () => await ProcessQueue());
    }

    public void RegisterHandler(Func<OpenViduEvent, Task> func)
    {
        _func = func;
    }

    public void StoreEvent(OpenViduEvent @event)
    {
        jobs.Writer.TryWrite(@event);
    }

    private async Task ProcessQueue()
    {
        while (!jobs.Reader.Completion.IsCompleted)
        {
            try
            {
                if (!await jobs.Reader.WaitToReadAsync())
                    break;
                if (_func == null)
                    break;

                OpenViduEvent? item;
                while (jobs.Reader.TryRead(out item))
                {
                    if (_func != null)
                        await _func(item);
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

}