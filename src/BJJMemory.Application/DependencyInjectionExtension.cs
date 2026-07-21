using BJJMemory.Application.UseCases.Usuarios.Register;
using BJJMemory.Communication.Usuarios.Requests;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BJJMemory.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddUseCases(services);
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUsuario, RegisterUsuario>();
        services.AddScoped<IValidator<RequestRegisterUsuario>, RegisterUsuarioValidator>();
    }
}
