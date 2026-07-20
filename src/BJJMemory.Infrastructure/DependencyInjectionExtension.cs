using BJJMemory.Domain.Repositories;
using BJJMemory.Domain.Repositories.Usuarios;
using BJJMemory.Infrastructure.DataAccess;
using BJJMemory.Infrastructure.DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BJJMemory.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddToken(services, configuration);
        AddDbContext(services, configuration);

        services.AddScoped<IPasswordEncripter, Security.Cryptography.BCrypt>();
        services.AddScoped<ILoggedUser, LoggedUser>();
    }

    private static void AddToken(IServiceCollection services, IConfiguration configuration)
    {
        var expirationInMinutes = uint.Parse(configuration["Settings:Jwt:ExpiresMinutes"]!);
        var signinKey = configuration["Settings:Jwt:SigninKey"];

        services.AddScoped<IAccessTokenGenerator>(config => new JwtTokenGenerator(expirationInMinutes, signinKey!));
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUsuarioUpdateOnlyRepository, UsuarioRepository>();
        services.AddScoped<IUsuarioReadOnlyRepository, UsuarioRepository>();
        services.AddScoped<IUsuarioWriteOnlyRepository, UsuarioRepository>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");

        services.AddDbContext<BJJMemoryDbContext>(config => config.UseNpgsql(connectionString));
    }
}
