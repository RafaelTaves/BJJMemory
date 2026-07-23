using BJJMemory.Application.Tests.Common.Fakers.Entities;
using BJJMemory.Application.Tests.Common.Fakers.Requests;
using BJJMemory.Application.Tests.Common.Fakes;
using BJJMemory.Application.Tests.Common.Helpers;
using BJJMemory.Application.UseCases.Categorias.Create;
using BJJMemory.Exception;
using BJJMemory.Exception.ExceptionsBase;
using Xunit;

namespace BJJMemory.Application.Tests.Categorias;

public class CreateCategoriaTests
{
    [Fact]
    public async Task Should_Create_Categoria_Without_Parent_And_Commit()
    {
        var (user, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var readOnlyRepository = new CategoriaReadOnlyRepositoryFake();
        var writeOnlyRepository = new CategoriaWriteOnlyRepositorySpy();
        var unitOfWork = new SpyUnitOfWork();
        var useCase = new CreateCategoria(
            loggedUser,
            readOnlyRepository,
            writeOnlyRepository,
            unitOfWork,
            new CreateCategoriaValidator());

        var request = RequestCreateCategoriaFaker.Generate(parentId: null);
        var response = await useCase.Execute(request);

        var categoriaAdicionada = Assert.Single(writeOnlyRepository.AddedCategorias);
        Assert.Equal(request.Nome, categoriaAdicionada.Nome);
        Assert.Equal(user.Id, categoriaAdicionada.UsuarioId);
        Assert.Null(categoriaAdicionada.ParentId);
        Assert.Equal(1, unitOfWork.CommitCalls);
        Assert.Equal(categoriaAdicionada.Id, response.Id);
        Assert.Equal(request.Nome, response.Nome);
        Assert.Null(response.ParentId);
    }

    [Fact]
    public async Task Should_Create_Categoria_With_Parent_And_Commit()
    {
        var (user, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var readOnlyRepository = new CategoriaReadOnlyRepositoryFake();
        var writeOnlyRepository = new CategoriaWriteOnlyRepositorySpy();
        var unitOfWork = new SpyUnitOfWork();
        var useCase = new CreateCategoria(
            loggedUser,
            readOnlyRepository,
            writeOnlyRepository,
            unitOfWork,
            new CreateCategoriaValidator());
        var parentId = Guid.NewGuid();

        readOnlyRepository.Categorias[parentId] = CategoriaFaker.Generate(usuarioId: user.Id, nome: "Guardas");
        readOnlyRepository.Categorias[parentId].Id = parentId;

        var request = RequestCreateCategoriaFaker.Generate(parentId: parentId);
        var response = await useCase.Execute(request);

        var categoriaAdicionada = Assert.Single(writeOnlyRepository.AddedCategorias);
        Assert.Equal(parentId, categoriaAdicionada.ParentId);
        Assert.Equal(1, unitOfWork.CommitCalls);
        Assert.Equal(parentId, response.ParentId);
    }

    [Fact]
    public async Task Should_Throw_When_Nome_Already_Exists()
    {
        var (_, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var readOnlyRepository = new CategoriaReadOnlyRepositoryFake
        {
            ExistCategoriaWithNomeResult = true
        };
        var useCase = new CreateCategoria(
            loggedUser,
            readOnlyRepository,
            new CategoriaWriteOnlyRepositorySpy(),
            new SpyUnitOfWork(),
            new CreateCategoriaValidator());
        var request = RequestCreateCategoriaFaker.Generate();

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => useCase.Execute(request));

        ValidationAssertHelper.AssertContainsErrors(exception, ResourceErrorMessages.CATEGORY_NAME_ALREADY_EXISTS);
    }

    [Fact]
    public async Task Should_Throw_When_Parent_Does_Not_Exist()
    {
        var (_, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var useCase = new CreateCategoria(
            loggedUser,
            new CategoriaReadOnlyRepositoryFake(),
            new CategoriaWriteOnlyRepositorySpy(),
            new SpyUnitOfWork(),
            new CreateCategoriaValidator());
        var request = RequestCreateCategoriaFaker.Generate(parentId: Guid.NewGuid());

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => useCase.Execute(request));

        ValidationAssertHelper.AssertContainsErrors(exception, ResourceErrorMessages.CATEGORY_PARENT_NOT_FOUND);
    }

    [Fact]
    public async Task Should_Throw_When_Request_Is_Invalid()
    {
        var (_, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var useCase = new CreateCategoria(
            loggedUser,
            new CategoriaReadOnlyRepositoryFake(),
            new CategoriaWriteOnlyRepositorySpy(),
            new SpyUnitOfWork(),
            new CreateCategoriaValidator());
        var request = RequestCreateCategoriaFaker.Generate(nome: string.Empty);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => useCase.Execute(request));

        ValidationAssertHelper.AssertContainsErrors(exception, ResourceErrorMessages.CATEGORY_NAME_REQUIRED);
    }
}
