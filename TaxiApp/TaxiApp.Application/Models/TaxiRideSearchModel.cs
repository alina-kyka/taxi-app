using System.Linq.Expressions;
using TaxiApp.Domain;

namespace TaxiApp.Application.Models;
public record TaxiRideSearchModel(Expression<Func<TaxiRide, bool>> Predicate, int Page, int PageSize);
