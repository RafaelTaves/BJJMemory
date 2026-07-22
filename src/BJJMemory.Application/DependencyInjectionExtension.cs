using BJJMemory.Application.UseCases.Login;
using BJJMemory.Application.UseCases.Usuarios.Get;
using BJJMemory.Application.UseCases.Usuarios.Register;
using BJJMemory.Application.UseCases.Usuarios.Update;
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
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IGetUsuario, GetUsuario>();
        services.AddScoped<IRegisterUsuario, RegisterUsuario>();
        services.AddScoped<IUpdateUsuario, UpdateUsuario>();
        services.AddScoped<IValidator<RequestRegisterUsuario>, RegisterUsuarioValidator>();
    }
}
