using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using TaxiApp.Application.Models;
using TaxiApp.Application.Services.Interfaces;

namespace TaxiApp.Presentation;
public static class UserInterface
{
    public static async Task StartAsync(IHost app, IConfiguration configuration)
    {
        const int DEFAULT_PAGE = 1;
        const int DEFAULT_PAGE_SIZE = 10;
        var INPUT_FILE_PATH = configuration["FilePaths:InputFilePath"];
        var OUTPUT_DUPLICATES_PATH = configuration["FilePaths:OutputDuplicatesPath"];

        bool isActive = true;

        using var scope = app.Services.CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<ITaxiRideService>();

        while (isActive)
        {
            ShowAvailableCommands();

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine(JsonSerializer.Serialize(await service.ImportAndSaveToDbAsync(new TaxiRidesImportRequestModel(INPUT_FILE_PATH!, OUTPUT_DUPLICATES_PATH!))));
                    break;
                case "2":
                    Console.WriteLine("Enter PULocationId");
                    int.TryParse(Console.ReadLine(), out var PULocationId);
                    Console.WriteLine(JsonSerializer.Serialize(await service.GetTaxiRidesByPULocationIdAsync(new TaxiRidesByPULocationIdRequestModel(PULocationId, DEFAULT_PAGE, DEFAULT_PAGE_SIZE))));
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
    private static void ShowAvailableCommands()
    {
        Console.WriteLine("1 - import from csv and save to db, 2 - GetTaxiRidesByPULocationIdAsync, 3 - GetLongestFaresTripDistanceAsync, 4 - GetLongestFaresTimeSpentTravelingAsync, 5 - GetLocationIdWithHighestAverageTipAsync, 0 - exit");
    }
}