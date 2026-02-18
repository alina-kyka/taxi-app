using TaxiApp.Application.Models;
using TaxiApp.Domain;

namespace TaxiApp.Application.Mapping;
public static class TaxiRideMappingExtension
{
    public static TaxiRideModel ToTaxiRideModel(this TaxiRide entity)
    {
        return new TaxiRideModel(
            entity.Id,
            entity.PickupDateTimeUtc,
            entity.DropoffDateTimeUtc,
            entity.PassengerCount,
            entity.TripDistance,
            entity.StoreAndFwdFlag,
            entity.PULocationId,
            entity.DOLocationId,
            entity.FareAmount,
            entity.TipAmount);
    }
}
