using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaxiApp.Application.Models;
using TaxiApp.Application.Repositories;
using TaxiApp.Domain;
using TaxiApp.Infrastructure.Contexts;

namespace TaxiApp.Infrastructure.Repositories;

public sealed class TaxiRideRepository : ITaxiRideRepository
{
    public readonly TaxiAppDbContext _context;
    public TaxiRideRepository(TaxiAppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(TaxiRide entity, CancellationToken ct = default)
    {
        await _context.TaxiRides.AddAsync(entity, ct);
    }

    public async Task<int> GetLocationIdWithHighestAverageTipAsync(CancellationToken ct = default)
    {
        var result = await _context.TaxiRides
            .GroupBy(x => x.PULocationId)
            .Select(x => new
            {
                PULocationId = x.Key,
                AverageTip = x.Average(t => t.TipAmount)
            })
            .OrderByDescending(x => x.AverageTip)
            .FirstAsync(ct);

        return result.PULocationId;
    }

    public async Task<IReadOnlyCollection<decimal>> GetLongestFaresTimeSpentTravelingAsync(int topAmount = 100, CancellationToken ct = default)
    {
        return await _context.TaxiRides
            .OrderByDescending(x => EF.Functions.DateDiffSecond(x.DropoffDateTimeUtc, x.PickupDateTimeUtc))
            .Take(topAmount)
            .Select(x => x.FareAmount)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<decimal>> GetLongestFaresTripDistanceAsync(int topAmount = 100, CancellationToken ct = default)
    {
        return await _context.TaxiRides
            .OrderByDescending(x => x.TripDistance)
            .Take(topAmount)
            .Select(x => x.FareAmount)
            .ToListAsync(ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyCollection<TaxiRide>> SearchAsync(TaxiRideSearchModel searchModel, CancellationToken ct = default)
    {
        return await _context.TaxiRides
            .Where(searchModel.Predicate)
            .OrderByDescending(x => x.PickupDateTimeUtc)
            .Skip((searchModel.Page - 1) * searchModel.PageSize)
            .Take(searchModel.PageSize)
            .ToListAsync(ct);
    }
}