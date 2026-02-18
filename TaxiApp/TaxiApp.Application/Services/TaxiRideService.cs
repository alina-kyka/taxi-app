using Microsoft.Extensions.Logging;
using System.Globalization;
using TaxiApp.Application.Mapping;
using TaxiApp.Application.Models;
using TaxiApp.Application.Repositories;
using TaxiApp.Application.Services.Interfaces;
using TaxiApp.Domain;

namespace TaxiApp.Application.Services;

public class TaxiRideService : ITaxiRideService
{
    private readonly ICsvService _csvService;
    private readonly ITaxiRideRepository _taxiRideRepository;
    private readonly ILogger<TaxiRideService> _logger;

    public TaxiRideService(ICsvService csvService, ITaxiRideRepository taxiRideRepository, ILogger<TaxiRideService> logger)
    {
        _csvService = csvService;
        _taxiRideRepository = taxiRideRepository;
        _logger = logger;
    }

    public async Task<int> GetLocationIdWithHighestAverageTipAsync(CancellationToken ct = default)
    {
        return await _taxiRideRepository.GetLocationIdWithHighestAverageTipAsync(ct);
    }

    public async Task<IReadOnlyCollection<decimal>> GetLongestFaresTimeSpentTravelingAsync(int topAmount = 100, CancellationToken ct = default)
    {
        return await _taxiRideRepository.GetLongestFaresTimeSpentTravelingAsync(topAmount, ct);
    }

    public async Task<IReadOnlyCollection<decimal>> GetLongestFaresTripDistanceAsync(int topAmount = 100, CancellationToken ct = default)
    {
        return await _taxiRideRepository.GetLongestFaresTripDistanceAsync(topAmount, ct);
    }

    public async Task<IReadOnlyCollection<TaxiRideModel>> GetTaxiRidesByPULocationIdAsync(TaxiRidesByPULocationIdRequestModel searchModel,
        CancellationToken ct = default)
    {
        var rides = await _taxiRideRepository.SearchAsync(
            new TaxiRideSearchModel(x => x.PULocationId == searchModel.PULocationId, searchModel.Page, searchModel.PageSize), 
            ct);

        return rides.Select(x => x.ToTaxiRideModel()).ToList();
    }

    public async Task<TaxiRideImportResult> ImportAndSaveToDbAsync(TaxiRidesImportRequestModel requestModel, CancellationToken ct = default)
    {
        var importResult = await SaveTaxiRidesToDbAndReturnDuplicatesAsync(
            _csvService.ImportEntitiesFromCsvAsync(requestModel.InputCsvFilePath, ct), ct);

        await _csvService.WriteDuplicatesToCsvAsync(
            new TaxiRideWriteDuplicatesRequestModel (requestModel.DuplicatesCsvPath, importResult.DuplicateRides), 
            ct);

        return importResult;
    }

    private async Task<TaxiRideImportResult> SaveTaxiRidesToDbAndReturnDuplicatesAsync(IAsyncEnumerable<TaxiRideCsvModel> rideModels, CancellationToken ct = default)
    {
        HashSet<TaxiRideKey> uniqueTaxiRides = [];
        List<TaxiRide> duplicateTaxiRides = [];

        try
        {
            await foreach (var rideModel in rideModels)
            {
                if (rideModel == null) continue;
                
                if (!TryParse(rideModel, out TaxiRide ride))
                {
                    throw new ArgumentException($"Failed to parse: {System.Text.Json.JsonSerializer.Serialize(rideModel)}");
                }

                if (!uniqueTaxiRides.Add(new TaxiRideKey(ride.PickupDateTimeUtc, ride.DropoffDateTimeUtc, ride.PassengerCount)))
                {
                    duplicateTaxiRides.Add(ride);
                }
                else
                {
                    await _taxiRideRepository.AddAsync(ride, ct);
                }
            }

            await _taxiRideRepository.SaveChangesAsync(ct);

            return new TaxiRideImportResult (duplicateTaxiRides, uniqueTaxiRides.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }

    private bool TryParse(TaxiRideCsvModel record, out TaxiRide trip)
    {
        trip = new TaxiRide();


        if (!byte.TryParse(record.PassengerCountRaw.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var passengerCount))
        {
            if (record.PassengerCountRaw.Trim() == string.Empty)
            {
                trip.PassengerCount = 0;
            }
            else
            {
                return false;
            }
        }

        if (!DateTime.TryParse(record.PickupDateTimeRaw.Trim(), CultureInfo.InvariantCulture, DateTimeStyles.None, out var pickup))
        {
            return false;
        }

        if (!DateTime.TryParse(record.DropoffDateTimeRaw.Trim(), CultureInfo.InvariantCulture, DateTimeStyles.None, out var dropoff))
        {
            return false;
        }

        if (!decimal.TryParse(record.TripDistanceRaw.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var tripDistance))
        {
            return false;
        }

        if (!int.TryParse(record.PULocationIdRaw.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var puLocationId))
        {
            return false;
        }

        if (!int.TryParse(record.DOLocationIdRaw.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var doLocationId))
        {
            return false;
        }

        if (!decimal.TryParse(record.FareAmountRaw.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var fareAmount))
        {
            return false;
        }

        if (!decimal.TryParse(record.TipAmountRaw.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var tipAmount))
        {
            return false;
        }

        var estZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        var pickupUtc = TimeZoneInfo.ConvertTimeToUtc(pickup, estZone);
        var dropoffUtc = TimeZoneInfo.ConvertTimeToUtc(dropoff, estZone);

        var storeAndFwdFlag = record.StoreAndFwdFlagRaw.Trim();
        storeAndFwdFlag = storeAndFwdFlag switch
        {
            "Y" => "Yes",
            "N" => "No",
            _ => storeAndFwdFlag
        };

        trip.PickupDateTimeUtc = pickupUtc;
        trip.DropoffDateTimeUtc = dropoffUtc;
        trip.PassengerCount = passengerCount;
        trip.TripDistance = tripDistance;
        trip.StoreAndFwdFlag = storeAndFwdFlag;
        trip.PULocationId = puLocationId;
        trip.DOLocationId = doLocationId;
        trip.FareAmount = fareAmount;
        trip.TipAmount = tipAmount;

        return true;
    }
}