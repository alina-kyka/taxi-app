using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaxiApp.Application.Repositories;
using TaxiApp.Application.Services;
using TaxiApp.Application.Services.Interfaces;
using TaxiApp.Infrastructure.Contexts;
using TaxiApp.Infrastructure.Repositories;

namespace TaxiApp.Presentation.Extensions;
public static  class DependencyInjectionExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICsvService, CsvService>();
        services.AddScoped<ITaxiRideService, TaxiRideService>();
    }
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TaxiAppDbContext>(options => options.UseSqlServer(configuration["ConnectionStrings:TaxiAppDbContext"]));
    }
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITaxiRideRepository, TaxiRideRepository>();
    }
}