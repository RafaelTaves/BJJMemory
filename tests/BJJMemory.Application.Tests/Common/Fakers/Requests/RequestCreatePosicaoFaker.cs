using Bogus;
using BJJMemory.Communication.Posicoes.Requests;

namespace BJJMemory.Application.Tests.Common.Fakers.Requests;

public static class RequestCreatePosicaoFaker
{
    private static readonly Faker Faker = new("pt_BR");

    public static RequestCreatePosicao Generate(
        Guid? categoriaId = null,
        string? titulo = null,
        string? descricao = null,
        string? audioLink = null,
        string? videoLink = null)
    {
        return new RequestCreatePosicao
        {
            CategoriaId = categoriaId ?? Guid.NewGuid(),
            Titulo = titulo ?? Faker.Lorem.Sentence(3),
            Descricao = descricao ?? Faker.Lorem.Paragraph(),
            AudioLink = audioLink ?? Faker.Internet.Url(),
            VideoLink = videoLink ?? Faker.Internet.Url()
        };
    }
}
