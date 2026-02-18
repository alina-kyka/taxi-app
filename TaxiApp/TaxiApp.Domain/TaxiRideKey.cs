namespace TaxiApp.Domain;
public record TaxiRideKey(DateTime PickupDateTimeUtc, DateTime DropoffDateTimeUtc, byte PassengerCount);
