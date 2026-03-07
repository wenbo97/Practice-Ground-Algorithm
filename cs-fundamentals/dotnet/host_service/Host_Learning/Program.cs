using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Host_Learning;

/// <summary>
/// Custom hosting program lifecycle.
/// 
/// Core class
/// <see cref="Microsoft.Extensions.Hosting.IHost"/>
/// <see cref="Microsoft.Extensions.Hosting.IHostApplicationLifetime"/>
/// </summary>
public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        await Host.CreateDefaultBuilder(args)
            .ConfigureServices(service =>
            {
                service.AddHostedService<DemoWorker>();
            })
            .RunConsoleAsync();
    }
}