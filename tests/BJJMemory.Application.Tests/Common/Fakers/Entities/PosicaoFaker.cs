using Bogus;
using BJJMemory.Domain.Entities;

namespace BJJMemory.Application.Tests.Common.Fakers.Entities;

public static class PosicaoFaker
{
    private static readonly Faker Faker = new("pt_BR");

    public static Posicao Generate(
        Guid? usuarioId = null,
        Guid? categoriaId = null,
        string? titulo = null,
        string? descricao = null,
        string? audioLink = null,
        string? videoLink = null)
    {
        return Posicao.Create(
            usuarioId ?? Guid.NewGuid(),
            categoriaId ?? Guid.NewGuid(),
            titulo ?? Faker.Lorem.Sentence(3),
            descricao ?? Faker.Lorem.Paragraph(),
            audioLink ?? Faker.Internet.Url(),
            videoLink ?? Faker.Internet.Url());
    }
}
