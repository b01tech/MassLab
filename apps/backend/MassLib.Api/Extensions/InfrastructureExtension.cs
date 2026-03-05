using MassLib.Api.Persistence;
using MassLib.Identity.Application;
using MassLib.Identity.Infrastructure;
using MassLib.Identity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MassLib.Api.Extensions;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IIdentityDbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<MassLib.Shared.Persistence.IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

        services.AddIdentityInfrastructure(configuration);
        services.AddIdentityApplication();

        return services;
    }

}

