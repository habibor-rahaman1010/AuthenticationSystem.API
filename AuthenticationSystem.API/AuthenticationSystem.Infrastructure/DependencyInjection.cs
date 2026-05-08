using AuthenticationSystem.Application.Interfaces.Repositories;
using AuthenticationSystem.Application.Interfaces.Services;
using AuthenticationSystem.Application.Services;
using AuthenticationSystem.Application.Settings;
using AuthenticationSystem.Infrastructure.Data;
using AuthenticationSystem.Infrastructure.Repositories;
using AuthenticationSystem.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Dapper context
        services.AddSingleton<DapperContext>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IModuleRepository, ModuleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();

        // Infrastructure services
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        // Application services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IModuleService, ModuleService>();
        services.AddScoped<IPermissionService, PermissionService>();

        // JWT settings
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        return services;
    }
}
