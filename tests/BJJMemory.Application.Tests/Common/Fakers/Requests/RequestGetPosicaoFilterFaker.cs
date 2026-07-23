using Bogus;
using BJJMemory.Communication.Posicoes.Requests;

namespace BJJMemory.Application.Tests.Common.Fakers.Requests;

public static class RequestGetPosicaoFilterFaker
{
    private static readonly Faker Faker = new("pt_BR");

    public static RequestGetPosicaoFilter Generate(
        string? nome = null,
        Guid? categoriaId = null,
        Guid? subcategoriaId = null)
    {
        return new RequestGetPosicaoFilter
        {
            Nome = nome ?? Faker.Lorem.Word(),
            CategoriaId = categoriaId,
            SubcategoriaId = subcategoriaId
        };
    }
}
