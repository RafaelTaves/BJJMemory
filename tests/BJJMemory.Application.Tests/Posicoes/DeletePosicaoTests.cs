using BJJMemory.Application.UseCases.Posicoes.Delete;
using BJJMemory.Application.Tests.Common.Fakes;
using BJJMemory.Application.Tests.Common.Helpers;
using BJJMemory.Exception;
using BJJMemory.Exception.ExceptionsBase;
using Xunit;

namespace BJJMemory.Application.Tests.Posicoes;

public class DeletePosicaoTests
{
    [Fact]
    public async Task Should_Delete_Posicao_And_Commit_When_Posicao_Exists()
    {
        var (user, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var posicaoWriteOnlyRepository = new PosicaoWriteOnlyRepositorySpy
        {
            DeleteResult = true
        };
        var unitOfWork = new SpyUnitOfWork();
        var useCase = new DeletePosicao(loggedUser, posicaoWriteOnlyRepository, unitOfWork);
        var posicaoId = Guid.NewGuid();

        await useCase.Execute(posicaoId);

        Assert.Equal(posicaoId, posicaoWriteOnlyRepository.DeleteReceivedPosicaoId);
        Assert.Equal(user.Id, posicaoWriteOnlyRepository.DeleteReceivedUsuarioId);
        Assert.Equal(1, unitOfWork.CommitCalls);
    }

    [Fact]
    public async Task Should_Throw_NotFound_When_Posicao_Does_Not_Exist()
    {
        var (_, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var posicaoWriteOnlyRepository = new PosicaoWriteOnlyRepositorySpy
        {
            DeleteResult = false
        };
        var useCase = new DeletePosicao(loggedUser, posicaoWriteOnlyRepository, new SpyUnitOfWork());

        var exception = await Assert.ThrowsAsync<NotFoundException>(() => useCase.Execute(Guid.NewGuid()));

        Assert.Equal(ResourceErrorMessages.POSITION_NOT_FOUND, exception.Message);
    }
}
