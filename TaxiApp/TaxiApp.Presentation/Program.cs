using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaxiApp.Application.Repositories;
using TaxiApp.Application.Services;
using TaxiApp.Infrastructure.Contexts;
using TaxiApp.Infrastructure.Repositories;
using System.Text.Json;

namespace TaxiApp.Presentation;

public class Program
{
    static async Task Main(string[] args)
    {
        const int DEFAULT_PAGE = 1;
        const int DEFAULT_PAGE_SIZE = 10;

        var builder = Host.CreateApplicationBuilder(args);

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        var INPUT_FILE_PATH = builder.Configuration["FilePaths:InputFilePath"];
        var OUTPUT_DUPLICATES_PATH = builder.Configuration["FilePaths:OutputDuplicatesPath"];

        builder.Services.AddDbContext<TaxiAppDbContext>(options =>options.UseSqlServer(builder.Configuration["ConnectionStrings:TaxiAppDbContext"]));
        builder.Services.AddScoped<ITaxiRideRepository, TaxiRideRepository>();
        builder.Services.AddScoped<ICsvService, CsvService>();
        builder.Services.AddScoped<ITaxiRideService, TaxiRideService>();

        var app = builder.Build();

        using var scope = app.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ITaxiRideService>();

        bool isActive = true;

        while (isActive)
        {
            Console.WriteLine("1 - import from csv and save to db, 2 - GetTaxiRidesByPULocationIdAsync, 3 - GetLongestFaresTripDistanceAsync, 4 - GetLongestFaresTimeSpentTravelingAsync, 5 - GetLocationIdWithHighestAverageTipAsync, 0 - exit");
            switch(Console.ReadLine()){
                case "1":
                    Console.WriteLine(JsonSerializer.Serialize(await service.ImportAndSaveToDbAsync(INPUT_FILE_PATH!, OUTPUT_DUPLICATES_PATH!)));
                    break;
                case "2":
                    Console.WriteLine("Enter PULocationId");
                    int.TryParse(Console.ReadLine(), out var PULocationId);
                    Console.WriteLine(JsonSerializer.Serialize( await service.GetTaxiRidesByPULocationIdAsync(PULocationId, DEFAULT_PAGE, DEFAULT_PAGE_SIZE)));
                    break;
                case "3":
                    Console.WriteLine(JsonSerializer.Serialize(await service.GetLongestFaresTripDistanceAsync()));
                    break;
                case "4":
                    Console.WriteLine(JsonSerializer.Serialize(await service.GetLongestFaresTimeSpentTravelingAsync()));
                    break;
                case "5":
                    Console.WriteLine(JsonSerializer.Serialize(await service.GetLocationIdWithHighestAverageTipAsync()));
                    break;
                case "0":
                    isActive = false;
                    break;
                default:
                    Console.WriteLine("Invalid operation.");
                    break;
            }
        } 
    }
}