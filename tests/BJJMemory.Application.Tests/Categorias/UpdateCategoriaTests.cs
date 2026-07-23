using BJJMemory.Application.Tests.Common.Fakers.Entities;
using BJJMemory.Application.Tests.Common.Fakers.Requests;
using BJJMemory.Application.Tests.Common.Fakes;
using BJJMemory.Application.Tests.Common.Helpers;
using BJJMemory.Application.UseCases.Categorias.Update;
using BJJMemory.Exception;
using BJJMemory.Exception.ExceptionsBase;
using Xunit;

namespace BJJMemory.Application.Tests.Categorias;

public class UpdateCategoriaTests
{
    [Fact]
    public async Task Should_Update_Categoria_And_Commit()
    {
        var (user, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var readOnlyRepository = new CategoriaReadOnlyRepositoryFake();
        var updateOnlyRepository = new CategoriaUpdateOnlyRepositorySpy();
        var unitOfWork = new SpyUnitOfWork();
        var useCase = new UpdateCategoria(loggedUser, readOnlyRepository, updateOnlyRepository, unitOfWork);
        var categoriaId = Guid.NewGuid();
        var categoria = CategoriaFaker.Generate(usuarioId: user.Id, nome: "Passagem");
        categoria.Id = categoriaId;
        readOnlyRepository.Categorias[categoriaId] = categoria;
        var request = RequestUpdateCategoriaFaker.Generate(nome: "Raspagens");

        await useCase.Execute(categoriaId, request);

        Assert.Equal(1, updateOnlyRepository.UpdateCalls);
        Assert.NotNull(updateOnlyRepository.UpdatedCategoria);
        Assert.Equal(categoriaId, updateOnlyRepository.UpdatedCategoria!.Id);
        Assert.Equal(request.Nome, updateOnlyRepository.UpdatedCategoria.Nome);
        Assert.Equal(1, unitOfWork.CommitCalls);
    }

    [Fact]
    public async Task Should_Throw_NotFound_When_Categoria_Does_Not_Exist()
    {
        var (_, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var useCase = new UpdateCategoria(
            loggedUser,
            new CategoriaReadOnlyRepositoryFake(),
            new CategoriaUpdateOnlyRepositorySpy(),
            new SpyUnitOfWork());

        await Assert.ThrowsAsync<NotFoundException>(
            () => useCase.Execute(Guid.NewGuid(), RequestUpdateCategoriaFaker.Generate()));
    }

    [Fact]
    public async Task Should_Throw_When_Nome_Already_Exists_For_Another_Categoria()
    {
        var (user, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var readOnlyRepository = new CategoriaReadOnlyRepositoryFake
        {
            ExistCategoriaWithNomeExceptIdResult = true
        };
        var categoriaId = Guid.NewGuid();
        var categoria = CategoriaFaker.Generate(usuarioId: user.Id);
        categoria.Id = categoriaId;
        readOnlyRepository.Categorias[categoriaId] = categoria;
        var useCase = new UpdateCategoria(
            loggedUser,
            readOnlyRepository,
            new CategoriaUpdateOnlyRepositorySpy(),
            new SpyUnitOfWork());

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(
            () => useCase.Execute(categoriaId, RequestUpdateCategoriaFaker.Generate()));

        ValidationAssertHelper.AssertContainsErrors(exception, ResourceErrorMessages.CATEGORY_NAME_ALREADY_EXISTS);
    }

    [Fact]
    public async Task Should_Throw_When_Request_Is_Invalid()
    {
        var (user, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var readOnlyRepository = new CategoriaReadOnlyRepositoryFake();
        var categoriaId = Guid.NewGuid();
        var categoria = CategoriaFaker.Generate(usuarioId: user.Id);
        categoria.Id = categoriaId;
        readOnlyRepository.Categorias[categoriaId] = categoria;
        var useCase = new UpdateCategoria(
            loggedUser,
            readOnlyRepository,
            new CategoriaUpdateOnlyRepositorySpy(),
            new SpyUnitOfWork());

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(
            () => useCase.Execute(categoriaId, RequestUpdateCategoriaFaker.Generate(nome: string.Empty)));

        ValidationAssertHelper.AssertContainsErrors(exception, ResourceErrorMessages.CATEGORY_NAME_REQUIRED);
    }
}
