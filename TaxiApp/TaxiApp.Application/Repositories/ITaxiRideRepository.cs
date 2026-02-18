using TaxiApp.Application.Models;
using TaxiApp.Domain;

namespace TaxiApp.Application.Repositories;

public interface ITaxiRideRepository
{
    Task AddAsync(TaxiRide entity, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
    Task<IReadOnlyCollection<TaxiRide>> SearchAsync(TaxiRideSearchModel searchModel, CancellationToken ct = default);
    Task<int> GetLocationIdWithHighestAverageTipAsync(CancellationToken ct = default);
    Task<IReadOnlyCollection<decimal>> GetLongestFaresTripDistanceAsync(int topAmount = 100, CancellationToken ct = default);
    Task<IReadOnlyCollection<decimal>> GetLongestFaresTimeSpentTravelingAsync(int topAmount = 100, CancellationToken ct = default);
}