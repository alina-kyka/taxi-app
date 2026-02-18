using TaxiApp.Application.Models;
using TaxiApp.Domain;

namespace TaxiApp.Application.Services.Interfaces;
public interface ICsvService
{
    IAsyncEnumerable<TaxiRideCsvModel> ImportEntitiesFromCsvAsync(string csvPath, CancellationToken ct = default);
    Task WriteDuplicatesToCsvAsync(TaxiRideWriteDuplicatesRequestModel requestModel, CancellationToken ct = default);
}
