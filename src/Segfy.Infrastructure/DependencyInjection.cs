using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Segfy.Application.Common.Interfaces;
using Segfy.Infrastructure.Persistence;
using Segfy.Infrastructure.Persistence.Repositories;

namespace Segfy.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SegfyDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("SegfyDatabase")));

        services.AddScoped<IApoliceRepository, ApoliceRepository>();
        services.AddScoped<ISinistroRepository, SinistroRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
