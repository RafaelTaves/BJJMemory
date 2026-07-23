using Bogus;
using BJJMemory.Domain.Entities;

namespace BJJMemory.Application.Tests.Common.Fakers.Entities;

public static class CategoriaFaker
{
    private static readonly Faker Faker = new("pt_BR");

    public static Categoria Generate(
        Guid? usuarioId = null,
        string? nome = null,
        Guid? parentId = null)
    {
        return Categoria.Create(
            usuarioId ?? Guid.NewGuid(),
            nome ?? Faker.Commerce.Categories(1).Single(),
            parentId);
    }
}
