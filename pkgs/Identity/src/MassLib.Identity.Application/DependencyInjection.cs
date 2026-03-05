using MassLib.Identity.Application.Commands.ActivateUser;
using MassLib.Identity.Application.Commands.ChangePassword;
using MassLib.Identity.Application.Commands.CreateUser;
using MassLib.Identity.Application.Commands.DeactivateUser;
using MassLib.Identity.Application.Commands.Login;
using MassLib.Identity.Application.Commands.UpdateUser;
using Microsoft.Extensions.DependencyInjection;

namespace MassLib.Identity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateUserHandler>();
        services.AddScoped<LoginHandler>();
        services.AddScoped<UpdateUserHandler>();
        services.AddScoped<ChangePasswordHandler>();
        services.AddScoped<DeactivateUserHandler>();
        services.AddScoped<ActivateUserHandler>();
        return services;
    }
}

