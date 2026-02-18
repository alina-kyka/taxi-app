using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Runtime.CompilerServices;
using TaxiApp.Application.Models;
using TaxiApp.Application.Services.Interfaces;
using TaxiApp.Domain;

namespace TaxiApp.Application.Services;

public class CsvService : ICsvService
{
    private readonly ILogger<CsvService> _logger;

    public CsvService(ILogger<CsvService> logger)
    {
        _logger = logger;
    }
    public async IAsyncEnumerable<TaxiRideCsvModel> ImportEntitiesFromCsvAsync(string csvPath, [EnumeratorCancellation] CancellationToken ct = default)
    {
        using var csvReader = new CsvReader(new StreamReader(csvPath), GetCsvConfiguration());

        await foreach (var record in csvReader.GetRecordsAsync<TaxiRideCsvModel>(ct))
        {
            yield return record;
        }
    }

    public async Task WriteDuplicatesToCsvAsync(TaxiRideWriteDuplicatesRequestModel requestModel, CancellationToken ct = default)
    {
        using var writer = new StreamWriter(requestModel.DuplicatesCsvPath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        csv.WriteHeader<TaxiRide>();
        csv.NextRecord();

        await csv.WriteRecordsAsync(requestModel.Duplicates, ct);
    }

    private CsvConfiguration GetCsvConfiguration()
    {
        return new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            ShouldSkipRecord = args => args.Row.Parser.Record?.All(string.IsNullOrWhiteSpace) == true,

            BadDataFound = args =>
            {
                _logger.LogError($"Format error at row {args.Context.Parser?.Row}");
            },

            ReadingExceptionOccurred = args =>
            {
                _logger.LogWarning($"Skipping row {args.Exception?.Context?.Parser?.Row} due to conversion error.");
                return false;
            }
        };
    }
}