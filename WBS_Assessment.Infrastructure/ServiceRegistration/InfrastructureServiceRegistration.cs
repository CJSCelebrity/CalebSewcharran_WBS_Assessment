using Microsoft.Extensions.DependencyInjection;
using WBS_Assessment.Application.Repositories;
using WBS_Assessment.Core.Models;
using WBS_Assessment.Infrastructure.Repositories;

namespace WBS_Assessment.Infrastructure.ServiceRegistration;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IBookingRepository, InMemoryBookingRepository>();
        services.AddSingleton<IUserRepository, InMemoryUserRepository>();

        services.AddSingleton<IRepository<Apartment>>(_ => new InMemoryRepository<Apartment>(SeedData.Apartments));
        services.AddSingleton<IRepository<Show>>(_ => new InMemoryRepository<Show>(SeedData.Shows));
        services.AddSingleton<IRepository<Vehicle>>(_ => new InMemoryRepository<Vehicle>(SeedData.Vehicles));

        return services;
    }
}