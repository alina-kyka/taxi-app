using TaxiApp.Domain;

namespace TaxiApp.Application.Models;
public record TaxiRideImportResult(IReadOnlyCollection<TaxiRide> DuplicateRides, int UniqueRidesCount);
