namespace TaxiApp.Domain;

public class TaxiRide
{
    public TaxiRide()
    {
        Id = Guid.NewGuid().ToString();
    }

    public string Id { get; set; }
    public DateTime PickupDateTimeUtc { get; set; }
    public DateTime DropoffDateTimeUtc { get; set; }
    public byte PassengerCount { get; set; }
    public decimal TripDistance { get; set; }
    public string StoreAndFwdFlag { get; set; } = string.Empty;
    public int PULocationId { get; set; }
    public int DOLocationId { get; set; }
    public decimal FareAmount { get; set; }
    public decimal TipAmount { get; set; }
}