using Bogus;
using BJJMemory.Communication.Usuarios.Requests;

namespace BJJMemory.Application.Tests.Common.Fakers.Requests;

public static class RequestLoginFaker
{
    private static readonly Faker Faker = new("pt_BR");

    public static RequestLogin Generate(
        string? email = null,
        string? password = null)
    {
        return new RequestLogin
        {
            Email = email ?? Faker.Internet.Email(),
            Password = password ?? Faker.Internet.Password()
        };
    }
}
