using Microsoft.EntityFrameworkCore;
using TaxiApp.Domain;

namespace TaxiApp.Infrastructure.Contexts;
public class TaxiAppDbContext : DbContext
{
    public DbSet<TaxiRide> TaxiRides { get; set; }

    public TaxiAppDbContext(DbContextOptions<TaxiAppDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}