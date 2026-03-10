using MassLab.Identity.Domain.Interfaces;
using MassLab.Identity.Infrastructure.Persistence.Repositories;
using MassLab.Identity.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MassLab.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IEncrypter, Encrypter>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
