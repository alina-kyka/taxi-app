using TaxiApp.Application.Models;

namespace TaxiApp.Application.Services.Interfaces;
public interface ITaxiRideService
{
    Task<TaxiRideImportResult> ImportAndSaveToDbAsync(TaxiRidesImportRequestModel requestModel, CancellationToken ct = default);
    Task<int> GetLocationIdWithHighestAverageTipAsync(CancellationToken ct = default);
    Task<IReadOnlyCollection<decimal>> GetLongestFaresTripDistanceAsync(int topAmount = 100, CancellationToken ct = default);
    Task<IReadOnlyCollection<decimal>> GetLongestFaresTimeSpentTravelingAsync(int topAmount = 100, CancellationToken ct = default);
    Task<IReadOnlyCollection<TaxiRideModel>> GetTaxiRidesByPULocationIdAsync(TaxiRidesByPULocationIdRequestModel searchModel, CancellationToken ct = default);
}
