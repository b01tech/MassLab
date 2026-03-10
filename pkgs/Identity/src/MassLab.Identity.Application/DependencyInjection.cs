using MassLab.Identity.Application.Commands.ActivateUser;
using MassLab.Identity.Application.Commands.ChangePassword;
using MassLab.Identity.Application.Commands.CreateUser;
using MassLab.Identity.Application.Commands.DeactivateUser;
using MassLab.Identity.Application.Commands.Login;
using MassLab.Identity.Application.Commands.RefreshToken;
using MassLab.Identity.Application.Commands.UpdateUser;
using MassLab.Identity.Application.Queries.GetUsers;
using Microsoft.Extensions.DependencyInjection;

namespace MassLab.Identity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateUserHandler>();
        services.AddScoped<LoginHandler>();
        services.AddScoped<RefreshTokenHandler>();
        services.AddScoped<UpdateUserHandler>();
        services.AddScoped<ChangePasswordHandler>();
        services.AddScoped<DeactivateUserHandler>();
        services.AddScoped<ActivateUserHandler>();
        services.AddScoped<GetUsersHandler>();
        return services;
    }
}

