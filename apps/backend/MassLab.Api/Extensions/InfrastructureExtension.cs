using MassLab.Api.Persistence;
using MassLab.Identity.Application;
using MassLab.Identity.Infrastructure;
using MassLab.Identity.Infrastructure.Persistence;
using MassLab.Registry.Application;
using MassLab.Registry.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MassLab.Api.Extensions;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IIdentityDbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<MassLab.Shared.Persistence.IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

        services.AddIdentityInfrastructure();
        services.AddIdentityApplication();
        services.AddRegistryInfrastructure();
        services.AddRegistryApplication();

        return services;
    }

}

