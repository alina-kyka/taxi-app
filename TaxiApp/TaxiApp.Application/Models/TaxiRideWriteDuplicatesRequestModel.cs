using TaxiApp.Domain;

namespace TaxiApp.Application.Models;
public record TaxiRideWriteDuplicatesRequestModel(string DuplicatesCsvPath, IReadOnlyCollection<TaxiRide> Duplicates);
