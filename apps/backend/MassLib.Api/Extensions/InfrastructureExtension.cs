using MassLib.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MassLib.Api.Extensions;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }

}
