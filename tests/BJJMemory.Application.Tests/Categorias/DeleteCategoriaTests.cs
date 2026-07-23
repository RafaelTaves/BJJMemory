using BJJMemory.Application.Tests.Common.Fakes;
using BJJMemory.Application.Tests.Common.Helpers;
using BJJMemory.Application.UseCases.Categorias.Delete;
using BJJMemory.Exception.ExceptionsBase;
using Xunit;

namespace BJJMemory.Application.Tests.Categorias;

public class DeleteCategoriaTests
{
    [Fact]
    public async Task Should_Delete_Categoria_And_Commit()
    {
        var (user, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var writeOnlyRepository = new CategoriaWriteOnlyRepositorySpy
        {
            DeleteResult = true
        };
        var unitOfWork = new SpyUnitOfWork();
        var useCase = new DeleteCategoria(loggedUser, writeOnlyRepository, unitOfWork);
        var categoriaId = Guid.NewGuid();

        await useCase.Execute(categoriaId);

        Assert.Equal(categoriaId, writeOnlyRepository.ReceivedDeleteCategoriaId);
        Assert.Equal(user.Id, writeOnlyRepository.ReceivedDeleteUsuarioId);
        Assert.Equal(1, unitOfWork.CommitCalls);
    }

    [Fact]
    public async Task Should_Throw_NotFound_When_Categoria_Does_Not_Exist()
    {
        var (_, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var writeOnlyRepository = new CategoriaWriteOnlyRepositorySpy
        {
            DeleteResult = false
        };
        var unitOfWork = new SpyUnitOfWork();
        var useCase = new DeleteCategoria(loggedUser, writeOnlyRepository, unitOfWork);

        await Assert.ThrowsAsync<NotFoundException>(() => useCase.Execute(Guid.NewGuid()));
        Assert.Equal(0, unitOfWork.CommitCalls);
    }
}
