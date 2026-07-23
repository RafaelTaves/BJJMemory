using Bogus;
using BJJMemory.Communication.Categorias.Requests;

namespace BJJMemory.Application.Tests.Common.Fakers.Requests;

public static class RequestCreateCategoriaFaker
{
    private static readonly Faker Faker = new("pt_BR");

    public static RequestCreateCategoria Generate(
        string? nome = null,
        Guid? parentId = null)
    {
        return new RequestCreateCategoria
        {
            Nome = nome ?? Faker.Commerce.Categories(1).Single(),
            ParentId = parentId
        };
    }
}
