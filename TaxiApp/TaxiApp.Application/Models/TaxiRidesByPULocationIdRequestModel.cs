namespace TaxiApp.Application.Models;
public record TaxiRidesByPULocationIdRequestModel(int PULocationId, int Page, int PageSize);
