using BJJMemory.Application.UseCases.Categorias.Create;
using BJJMemory.Application.UseCases.Categorias.Delete;
using BJJMemory.Application.UseCases.Categorias.Get;
using BJJMemory.Application.UseCases.Categorias.Update;
using BJJMemory.Application.UseCases.Login;
using BJJMemory.Application.UseCases.Usuarios.Get;
using BJJMemory.Application.UseCases.Usuarios.Register;
using BJJMemory.Application.UseCases.Usuarios.Update;
using BJJMemory.Communication.Categorias.Requests;
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
        services.AddScoped<ICreateCategoria, CreateCategoria>();
        services.AddScoped<IGetCategoria, GetCategoria>();
        services.AddScoped<IUpdateCategoria, UpdateCategoria>();
        services.AddScoped<IDeleteCategoria, DeleteCategoria>();
        services.AddScoped<IGetUsuario, GetUsuario>();
        services.AddScoped<IRegisterUsuario, RegisterUsuario>();
        services.AddScoped<IUpdateUsuario, UpdateUsuario>();
        services.AddScoped<IValidator<RequestCreateCategoria>, CreateCategoriaValidator>();
        services.AddScoped<IValidator<RequestRegisterUsuario>, RegisterUsuarioValidator>();
    }
}
