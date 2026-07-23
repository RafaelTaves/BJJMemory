using Bogus;
using BJJMemory.Communication.Categorias.Requests;

namespace BJJMemory.Application.Tests.Common.Fakers.Requests;

public static class RequestUpdateCategoriaFaker
{
    private static readonly Faker Faker = new("pt_BR");

    public static RequestUpdateCategoria Generate(string? nome = null)
    {
        return new RequestUpdateCategoria
        {
            Nome = nome ?? Faker.Commerce.Categories(1).Single()
        };
    }
}
