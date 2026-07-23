using BJJMemory.Application.UseCases.Posicoes.Update;
using BJJMemory.Application.Tests.Common.Fakers.Entities;
using BJJMemory.Application.Tests.Common.Fakers.Requests;
using BJJMemory.Application.Tests.Common.Fakes;
using BJJMemory.Application.Tests.Common.Helpers;
using BJJMemory.Domain.Services.Posicoes.Midia;
using BJJMemory.Exception;
using BJJMemory.Exception.ExceptionsBase;
using Xunit;

namespace BJJMemory.Application.Tests.Posicoes;

public class UpdatePosicaoTests
{
    [Fact]
    public async Task Should_Update_Posicao_And_Commit_When_Request_Is_Valid()
    {
        var (user, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var categoriaReadOnlyRepository = new CategoriaReadOnlyRepositoryFake();
        var posicaoReadOnlyRepository = new PosicaoReadOnlyRepositorySpy();
        var posicaoUpdateOnlyRepository = new PosicaoUpdateOnlyRepositorySpy();
        var unitOfWork = new SpyUnitOfWork();
        var posicaoMidiaFacade = new PosicaoMidiaFacadeFake
        {
            ProcessResult = new PosicaoMidiaResult
            {
                AudioLink = "https://cdn.bjjmemory.com/novo-audio.mp3",
                VideoLink = "https://cdn.bjjmemory.com/novo-video.mp4"
            }
        };
        var useCase = new UpdatePosicao(
            loggedUser,
            categoriaReadOnlyRepository,
            posicaoReadOnlyRepository,
            posicaoUpdateOnlyRepository,
            unitOfWork,
            posicaoMidiaFacade);

        var posicaoId = Guid.NewGuid();
        var categoriaId = Guid.NewGuid();
        var request = RequestUpdatePosicaoFaker.Generate(categoriaId: categoriaId, titulo: "Novo título", descricao: "Nova descrição");
        var posicao = PosicaoFaker.Generate(usuarioId: user.Id);
        categoriaReadOnlyRepository.Categorias[categoriaId] = CategoriaFaker.Generate(usuarioId: user.Id);
        posicaoReadOnlyRepository.PosicoesById[posicaoId] = posicao;

        await useCase.Execute(posicaoId, request);

        Assert.Equal(1, posicaoUpdateOnlyRepository.UpdateCalls);
        Assert.Equal(1, unitOfWork.CommitCalls);
        Assert.Equal(categoriaId, posicao.CategoriaId);
        Assert.Equal("Novo título", posicao.Titulo);
        Assert.Equal("Nova descrição", posicao.Descricao);
        Assert.Equal("https://cdn.bjjmemory.com/novo-audio.mp3", posicao.AudioLink);
        Assert.Equal("https://cdn.bjjmemory.com/novo-video.mp4", posicao.VideoLink);
    }

    [Fact]
    public async Task Should_Throw_NotFound_When_Posicao_Does_Not_Exist()
    {
        var (_, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var useCase = new UpdatePosicao(
            loggedUser,
            new CategoriaReadOnlyRepositoryFake(),
            new PosicaoReadOnlyRepositorySpy(),
            new PosicaoUpdateOnlyRepositorySpy(),
            new SpyUnitOfWork(),
            new PosicaoMidiaFacadeFake());

        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            useCase.Execute(Guid.NewGuid(), RequestUpdatePosicaoFaker.Generate()));

        Assert.Equal(ResourceErrorMessages.POSITION_NOT_FOUND, exception.Message);
    }

    [Fact]
    public async Task Should_Throw_When_Categoria_Does_Not_Exist()
    {
        var (user, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var posicaoReadOnlyRepository = new PosicaoReadOnlyRepositorySpy();
        var posicaoId = Guid.NewGuid();
        posicaoReadOnlyRepository.PosicoesById[posicaoId] = PosicaoFaker.Generate(usuarioId: user.Id);
        var request = RequestUpdatePosicaoFaker.Generate(categoriaId: Guid.NewGuid());
        var useCase = new UpdatePosicao(
            loggedUser,
            new CategoriaReadOnlyRepositoryFake(),
            posicaoReadOnlyRepository,
            new PosicaoUpdateOnlyRepositorySpy(),
            new SpyUnitOfWork(),
            new PosicaoMidiaFacadeFake());

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => useCase.Execute(posicaoId, request));

        ValidationAssertHelper.AssertContainsErrors(exception, ResourceErrorMessages.POSITION_CATEGORY_NOT_FOUND);
    }

    [Fact]
    public async Task Should_Aggregate_Midia_Errors_From_Facade()
    {
        var (user, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var categoriaId = Guid.NewGuid();
        var posicaoId = Guid.NewGuid();
        var categoriaReadOnlyRepository = new CategoriaReadOnlyRepositoryFake();
        var posicaoReadOnlyRepository = new PosicaoReadOnlyRepositorySpy();
        var posicaoMidiaFacade = new PosicaoMidiaFacadeFake
        {
            ProcessResult = new PosicaoMidiaResult
            {
                Errors =
                [
                    ResourceErrorMessages.POSITION_INVALID_AUDIO_LINK,
                    ResourceErrorMessages.POSITION_INVALID_VIDEO_LINK
                ]
            }
        };
        var useCase = new UpdatePosicao(
            loggedUser,
            categoriaReadOnlyRepository,
            posicaoReadOnlyRepository,
            new PosicaoUpdateOnlyRepositorySpy(),
            new SpyUnitOfWork(),
            posicaoMidiaFacade);
        categoriaReadOnlyRepository.Categorias[categoriaId] = CategoriaFaker.Generate(usuarioId: user.Id);
        posicaoReadOnlyRepository.PosicoesById[posicaoId] = PosicaoFaker.Generate(usuarioId: user.Id);
        var request = RequestUpdatePosicaoFaker.Generate(categoriaId: categoriaId);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => useCase.Execute(posicaoId, request));

        ValidationAssertHelper.AssertContainsErrors(
            exception,
            ResourceErrorMessages.POSITION_INVALID_AUDIO_LINK,
            ResourceErrorMessages.POSITION_INVALID_VIDEO_LINK);
    }
}
