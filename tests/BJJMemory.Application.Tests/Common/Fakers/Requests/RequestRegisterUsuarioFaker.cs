using Bogus;
using BJJMemory.Communication.Usuarios.Requests;

namespace BJJMemory.Application.Tests.Common.Fakers.Requests;

public static class RequestRegisterUsuarioFaker
{
    private static readonly Faker Faker = new("pt_BR");

    public static RequestRegisterUsuario Generate(
        string? username = null,
        string? email = null,
        string? password = null)
    {
        return new RequestRegisterUsuario
        {
            Username = username ?? Faker.Internet.UserName(),
            Email = email ?? Faker.Internet.Email(),
            Password = password ?? Faker.Internet.Password()
        };
    }
}
