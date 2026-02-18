using System.Linq.Expressions;
using TaxiApp.Domain;

namespace TaxiApp.Application.Repositories;

public interface ITaxiRideRepository
{
    Task AddAsync(TaxiRide entity, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
    Task<IReadOnlyCollection<TaxiRide>> SearchAsync(Expression<Func<TaxiRide, bool>> predicate, int page, int pageSize, CancellationToken ct = default);
    public Task<int> GetLocationIdWithHighestAverageTipAsync(CancellationToken ct = default);
    public Task<IReadOnlyCollection<decimal>> GetLongestFaresTripDistanceAsync(int topAmount = 100, CancellationToken ct = default);
    public Task<IReadOnlyCollection<decimal>> GetLongestFaresTimeSpentTravelingAsync(int topAmount = 100, CancellationToken ct = default);
}