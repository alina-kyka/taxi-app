using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaxiApp.Domain;

namespace TaxiApp.Infrastructure.EntityConfiguration;

public class TaxiRideEntityConfiguration : IEntityTypeConfiguration<TaxiRide>
{
    public void Configure(EntityTypeBuilder<TaxiRide> builder)
    {
        builder.HasKey(x => x.Id);
    }
}