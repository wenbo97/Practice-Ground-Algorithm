using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Host_Learning;

public class DemoWorker : BackgroundService
{
    private readonly ILogger<DemoWorker> logger;
    private readonly IHostApplicationLifetime applicationLifetime;

    public DemoWorker(ILogger<DemoWorker> logger, IHostApplicationLifetime applicationLifetime)
    {
        this.logger = logger;
        this.applicationLifetime = applicationLifetime;
    }


    public override Task StartAsync(CancellationToken cancellationToken)
    {
        this.applicationLifetime.ApplicationStarted.Register(() =>
        {
            this.logger.LogInformation("ApplicationStarted fired");
        });

        this.applicationLifetime.ApplicationStopping.Register(() =>
        {
            this.logger.LogInformation("ApplicationStopping fired");
        });

        this.applicationLifetime.ApplicationStopped.Register(() =>
        {
            this.logger.LogInformation("ApplicationStopped fired");
        });

        return base.StartAsync(cancellationToken);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.logger.LogInformation("Worker Start.");
        while (!stoppingToken.IsCancellationRequested)
        {
            this.logger.LogInformation("Working...");

            await Task.Delay((int)TimeSpan.FromSeconds(3).TotalMilliseconds);
        }

        this.logger.LogInformation("Work Stop.");
    }
}