using Bogus;
using BJJMemory.Communication.Usuarios.Requests;

namespace BJJMemory.Application.Tests.Common.Fakers.Requests;

public static class RequestUpdateUsuarioFaker
{
    private static readonly Faker Faker = new("pt_BR");

    public static RequestUpdateUsuario Generate(
        string? username = null,
        string? email = null)
    {
        return new RequestUpdateUsuario
        {
            Username = username ?? Faker.Internet.UserName(),
            Email = email ?? Faker.Internet.Email()
        };
    }
}
