using Bogus;
using BJJMemory.Domain.Entities;

namespace BJJMemory.Application.Tests.Common.Fakers.Entities;

public static class UsuarioFaker
{
    private static readonly Faker Faker = new("pt_BR");

    public static Usuario Generate(
        string? username = null,
        string? email = null,
        string? hashedPassword = null)
    {
        return Usuario.Create(
            username ?? Faker.Internet.UserName(),
            email ?? Faker.Internet.Email(),
            hashedPassword ?? Faker.Internet.Password());
    }
}
