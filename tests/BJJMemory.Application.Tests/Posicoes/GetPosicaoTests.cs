using BJJMemory.Application.UseCases.Posicoes.Get;
using BJJMemory.Communication.Posicoes.Requests;
using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Repositories.Categorias;
using BJJMemory.Domain.Repositories.Posicoes;
using BJJMemory.Domain.Services.LoggedUser;
using BJJMemory.Exception.ExceptionsBase;
using Xunit;

namespace BJJMemory.Application.Tests.Posicoes;

public class GetPosicaoTests
{
    [Fact]
    public async Task Should_Prioritize_Subcategoria_Filter_When_Both_Are_Informed()
    {
        var user = Usuario.Create("rafael", "rafael@bjjmemory.com", "hashed");
        var loggedUser = new LoggedUserFake(user);
        var categoriaReadOnlyRepository = new CategoriaReadOnlyRepositoryFake();
        var posicaoReadOnlyRepository = new PosicaoReadOnlyRepositoryFake();
        var useCase = new GetPosicao(loggedUser, posicaoReadOnlyRepository, categoriaReadOnlyRepository);
        var categoriaId = Guid.NewGuid();
        var subcategoriaId = Guid.NewGuid();

        categoriaReadOnlyRepository.Categorias[subcategoriaId] = Categoria.Create(user.Id, "Subcategoria", categoriaId);

        await useCase.Execute(new RequestGetPosicaoFilter
        {
            Nome = "arm",
            CategoriaId = categoriaId,
            SubcategoriaId = subcategoriaId
        });

        Assert.Equal(subcategoriaId, posicaoReadOnlyRepository.CategoriaIdRecebida);
        Assert.False(posicaoReadOnlyRepository.IncluirSubcategoriasRecebido);
    }

    [Fact]
    public async Task Should_Throw_When_Filtered_Category_Does_Not_Exist()
    {
        var user = Usuario.Create("rafael", "rafael@bjjmemory.com", "hashed");
        var loggedUser = new LoggedUserFake(user);
        var categoriaReadOnlyRepository = new CategoriaReadOnlyRepositoryFake();
        var posicaoReadOnlyRepository = new PosicaoReadOnlyRepositoryFake();
        var useCase = new GetPosicao(loggedUser, posicaoReadOnlyRepository, categoriaReadOnlyRepository);

        await Assert.ThrowsAsync<ErrorOnValidationException>(() => useCase.Execute(new RequestGetPosicaoFilter
        {
            CategoriaId = Guid.NewGuid()
        }));
    }

    private class LoggedUserFake : ILoggedUser
    {
        private readonly Usuario _usuario;

        public LoggedUserFake(Usuario usuario)
        {
            _usuario = usuario;
        }

        public Task<Usuario> Get()
        {
            return Task.FromResult(_usuario);
        }
    }

    private class CategoriaReadOnlyRepositoryFake : ICategoriaReadOnlyRepository
    {
        public Dictionary<Guid, Categoria> Categorias { get; } = [];

        public Task<bool> ExistCategoriaWithNome(Guid usuarioId, string nome)
        {
            return Task.FromResult(false);
        }

        public Task<bool> ExistCategoriaWithNomeExceptId(Guid usuarioId, string nome, Guid categoriaId)
        {
            return Task.FromResult(false);
        }

        public Task<IList<Categoria>> GetAllByUsuarioId(Guid usuarioId)
        {
            return Task.FromResult<IList<Categoria>>([]);
        }

        public Task<Categoria?> GetById(Guid categoriaId, Guid usuarioId)
        {
            Categorias.TryGetValue(categoriaId, out var categoria);
            return Task.FromResult(categoria);
        }
    }

    private class PosicaoReadOnlyRepositoryFake : IPosicaoReadOnlyRepository
    {
        public Guid? CategoriaIdRecebida { get; private set; }

        public bool IncluirSubcategoriasRecebido { get; private set; }

        public Task<IList<Posicao>> GetAllByFilters(Guid usuarioId, string? nome, Guid? categoriaId, bool incluirSubcategorias)
        {
            CategoriaIdRecebida = categoriaId;
            IncluirSubcategoriasRecebido = incluirSubcategorias;
            return Task.FromResult<IList<Posicao>>([]);
        }

        public Task<Posicao?> GetById(Guid posicaoId, Guid usuarioId)
        {
            return Task.FromResult<Posicao?>(null);
        }
    }
}
