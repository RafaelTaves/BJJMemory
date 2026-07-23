using BJJMemory.Application.Tests.Common.Fakers.Entities;
using BJJMemory.Application.Tests.Common.Fakes;
using BJJMemory.Application.Tests.Common.Helpers;
using BJJMemory.Application.UseCases.Categorias.Get;
using Xunit;

namespace BJJMemory.Application.Tests.Categorias;

public class GetCategoriaTests
{
    [Fact]
    public async Task Should_Build_Tree_And_Order_Categorias_By_Nome()
    {
        var (user, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var readOnlyRepository = new CategoriaReadOnlyRepositoryFake();
        var useCase = new GetCategoria(loggedUser, readOnlyRepository);

        var rootAId = Guid.NewGuid();
        var rootBId = Guid.NewGuid();
        var rootCId = Guid.NewGuid();
        var childAId = Guid.NewGuid();
        var childBId = Guid.NewGuid();

        readOnlyRepository.CategoriasByUsuarioIdResult =
        [
            CreateCategoria(user.Id, rootCId, "Zeta", null),
            CreateCategoria(user.Id, childBId, "Kimura", rootAId),
            CreateCategoria(user.Id, rootAId, "Armbar", null),
            CreateCategoria(user.Id, childAId, "Americana", rootAId),
            CreateCategoria(user.Id, rootBId, "De La Riva", null)
        ];

        var result = await useCase.Execute();

        Assert.Collection(result,
            categoria => Assert.Equal("Armbar", categoria.Nome),
            categoria => Assert.Equal("De La Riva", categoria.Nome),
            categoria => Assert.Equal("Zeta", categoria.Nome));
        Assert.Collection(result[0].Subcategorias,
            categoria => Assert.Equal("Americana", categoria.Nome),
            categoria => Assert.Equal("Kimura", categoria.Nome));
    }

    [Fact]
    public async Task Should_Build_Correct_Subcategoria_Hierarchy()
    {
        var (user, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var readOnlyRepository = new CategoriaReadOnlyRepositoryFake();
        var useCase = new GetCategoria(loggedUser, readOnlyRepository);

        var rootId = Guid.NewGuid();
        var level1Id = Guid.NewGuid();
        var level2Id = Guid.NewGuid();

        readOnlyRepository.CategoriasByUsuarioIdResult =
        [
            CreateCategoria(user.Id, rootId, "Passagem", null),
            CreateCategoria(user.Id, level1Id, "Joelho na barriga", rootId),
            CreateCategoria(user.Id, level2Id, "Finalização", level1Id)
        ];

        var result = await useCase.Execute();

        var root = Assert.Single(result);
        Assert.Equal(rootId, root.Id);
        var level1 = Assert.Single(root.Subcategorias);
        Assert.Equal(level1Id, level1.Id);
        var level2 = Assert.Single(level1.Subcategorias);
        Assert.Equal(level2Id, level2.Id);
        Assert.Empty(level2.Subcategorias);
    }

    private static BJJMemory.Domain.Entities.Categoria CreateCategoria(Guid usuarioId, Guid categoriaId, string nome, Guid? parentId)
    {
        var categoria = CategoriaFaker.Generate(usuarioId: usuarioId, nome: nome, parentId: parentId);
        categoria.Id = categoriaId;
        return categoria;
    }
}
