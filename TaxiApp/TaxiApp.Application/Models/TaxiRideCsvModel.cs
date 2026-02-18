using CsvHelper.Configuration.Attributes;

namespace TaxiApp.Application.Models;

public class TaxiRideCsvModel
{
    [Name("tpep_pickup_datetime")]
    public string PickupDateTimeRaw { get; set; } = string.Empty;

    [Name("tpep_dropoff_datetime")]
    public string DropoffDateTimeRaw { get; set; } = string.Empty;

    [Name("passenger_count")]
    public string PassengerCountRaw { get; set; } = string.Empty;

    [Name("trip_distance")]
    public string TripDistanceRaw { get; set; } = string.Empty;

    [Name("store_and_fwd_flag")]
    public string StoreAndFwdFlagRaw { get; set; } = string.Empty;

    [Name("PULocationID")]
    public string PULocationIdRaw { get; set; } = string.Empty;

    [Name("DOLocationID")]
    public string DOLocationIdRaw { get; set; } = string.Empty;

    [Name("fare_amount")]
    public string FareAmountRaw { get; set; } = string.Empty;

    [Name("tip_amount")]
    public string TipAmountRaw { get; set; } = string.Empty;
}

