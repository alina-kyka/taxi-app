using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaxiApp.Presentation.Extensions;

namespace TaxiApp.Presentation;

public class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
        
        builder.Services.AddDbContext(builder.Configuration);
        builder.Services.AddServices();
        builder.Services.AddRepositories();

        var app = builder.Build();

        await UserInterface.StartAsync(app, builder.Configuration);
    }
}