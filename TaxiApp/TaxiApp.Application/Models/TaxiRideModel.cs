namespace TaxiApp.Application.Models;
public record TaxiRideModel(
    string Id, 
    DateTime PickupDateTimeUtc, 
    DateTime DropoffDateTimeUtc, 
    byte PassengerCount,
    decimal TripDistance,
    string StoreAndFwdFlag, 
    int PULocationId, 
    int DOLocationId,
    decimal FareAmount, 
    decimal TipAmount);