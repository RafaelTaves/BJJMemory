using BJJMemory.Application.UseCases.Posicoes.Create;
using BJJMemory.Application.Tests.Common.Fakers.Entities;
using BJJMemory.Application.Tests.Common.Fakers.Requests;
using BJJMemory.Application.Tests.Common.Fakes;
using BJJMemory.Application.Tests.Common.Helpers;
using BJJMemory.Domain.Services.Posicoes.Midia;
using BJJMemory.Exception;
using BJJMemory.Exception.ExceptionsBase;
using Xunit;

namespace BJJMemory.Application.Tests.Posicoes;

public class CreatePosicaoTests
{
    [Fact]
    public async Task Should_Create_Posicao_And_Commit_When_Request_Is_Valid()
    {
        var (user, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var categoriaReadOnlyRepository = new CategoriaReadOnlyRepositoryFake();
        var posicaoWriteOnlyRepository = new PosicaoWriteOnlyRepositorySpy();
        var unitOfWork = new SpyUnitOfWork();
        var posicaoMidiaFacade = new PosicaoMidiaFacadeFake
        {
            ProcessResult = new PosicaoMidiaResult
            {
                AudioLink = "https://cdn.bjjmemory.com/audio-processado.mp3",
                VideoLink = "https://cdn.bjjmemory.com/video-processado.mp4"
            }
        };
        var useCase = new CreatePosicao(
            loggedUser,
            categoriaReadOnlyRepository,
            posicaoWriteOnlyRepository,
            unitOfWork,
            new CreatePosicaoValidator(),
            posicaoMidiaFacade);

        var categoriaId = Guid.NewGuid();
        categoriaReadOnlyRepository.Categorias[categoriaId] = CategoriaFaker.Generate(usuarioId: user.Id, nome: "Guarda");
        var request = RequestCreatePosicaoFaker.Generate(categoriaId: categoriaId);

        var response = await useCase.Execute(request);

        Assert.Equal(1, posicaoWriteOnlyRepository.AddCalls);
        Assert.Equal(1, unitOfWork.CommitCalls);
        var addedPosicao = Assert.IsType<BJJMemory.Domain.Entities.Posicao>(posicaoWriteOnlyRepository.AddedPosicao);
        Assert.Equal(categoriaId, addedPosicao.CategoriaId);
        Assert.Equal(user.Id, addedPosicao.UsuarioId);
        Assert.Equal(request.Titulo, addedPosicao.Titulo);
        Assert.Equal(request.Descricao, addedPosicao.Descricao);
        Assert.Equal("https://cdn.bjjmemory.com/audio-processado.mp3", addedPosicao.AudioLink);
        Assert.Equal("https://cdn.bjjmemory.com/video-processado.mp4", addedPosicao.VideoLink);
        Assert.Equal(addedPosicao.Id, response.Id);
    }

    [Fact]
    public async Task Should_Throw_When_Categoria_Does_Not_Exist()
    {
        var (_, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var useCase = new CreatePosicao(
            loggedUser,
            new CategoriaReadOnlyRepositoryFake(),
            new PosicaoWriteOnlyRepositorySpy(),
            new SpyUnitOfWork(),
            new CreatePosicaoValidator(),
            new PosicaoMidiaFacadeFake());
        var request = RequestCreatePosicaoFaker.Generate(categoriaId: Guid.NewGuid());

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => useCase.Execute(request));

        ValidationAssertHelper.AssertContainsErrors(exception, ResourceErrorMessages.POSITION_CATEGORY_NOT_FOUND);
    }

    [Fact]
    public async Task Should_Aggregate_Midia_Errors_From_Facade()
    {
        var (user, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var categoriaId = Guid.NewGuid();
        var categoriaReadOnlyRepository = new CategoriaReadOnlyRepositoryFake();
        categoriaReadOnlyRepository.Categorias[categoriaId] = CategoriaFaker.Generate(usuarioId: user.Id);
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
        var useCase = new CreatePosicao(
            loggedUser,
            categoriaReadOnlyRepository,
            new PosicaoWriteOnlyRepositorySpy(),
            new SpyUnitOfWork(),
            new CreatePosicaoValidator(),
            posicaoMidiaFacade);
        var request = RequestCreatePosicaoFaker.Generate(categoriaId: categoriaId);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => useCase.Execute(request));

        ValidationAssertHelper.AssertContainsErrors(
            exception,
            ResourceErrorMessages.POSITION_INVALID_AUDIO_LINK,
            ResourceErrorMessages.POSITION_INVALID_VIDEO_LINK);
    }

    [Fact]
    public async Task Should_Map_Processed_Audio_And_Video_On_Response()
    {
        var (user, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var categoriaReadOnlyRepository = new CategoriaReadOnlyRepositoryFake();
        var posicaoWriteOnlyRepository = new PosicaoWriteOnlyRepositorySpy();
        var unitOfWork = new SpyUnitOfWork();
        var categoriaId = Guid.NewGuid();
        var request = RequestCreatePosicaoFaker.Generate(
            categoriaId: categoriaId,
            audioLink: "https://input.bjjmemory.com/audio.mp3",
            videoLink: "https://input.bjjmemory.com/video.mp4");
        var posicaoMidiaFacade = new PosicaoMidiaFacadeFake
        {
            ProcessResult = new PosicaoMidiaResult
            {
                AudioLink = "https://processed.bjjmemory.com/audio.mp3",
                VideoLink = "https://processed.bjjmemory.com/video.mp4"
            }
        };
        var useCase = new CreatePosicao(
            loggedUser,
            categoriaReadOnlyRepository,
            posicaoWriteOnlyRepository,
            unitOfWork,
            new CreatePosicaoValidator(),
            posicaoMidiaFacade);

        categoriaReadOnlyRepository.Categorias[categoriaId] = CategoriaFaker.Generate(usuarioId: user.Id);
        var response = await useCase.Execute(request);

        Assert.Equal(request.AudioLink, posicaoMidiaFacade.ReceivedAudioLink);
        Assert.Equal(request.VideoLink, posicaoMidiaFacade.ReceivedVideoLink);
        Assert.Equal("https://processed.bjjmemory.com/audio.mp3", response.AudioLink);
        Assert.Equal("https://processed.bjjmemory.com/video.mp4", response.VideoLink);
        Assert.Equal(1, unitOfWork.CommitCalls);
        Assert.NotNull(posicaoWriteOnlyRepository.AddedPosicao);
    }
}
