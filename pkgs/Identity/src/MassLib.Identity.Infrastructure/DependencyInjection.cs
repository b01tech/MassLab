using MassLib.Identity.Domain.Interfaces;
using MassLib.Identity.Infrastructure.Persistence.Repositories;
using MassLib.Identity.Infrastructure.Services;
using MassLib.Shared.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MassLib.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEncrypter, Encrypter>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        // IUnitOfWork and DbContext should be provided by the host application (AppDbContext)
        // or a shared persistence module if configured differently.
        // Assuming AppDbContext is registered as Scoped in the API layer.

        return services;
    }
}
