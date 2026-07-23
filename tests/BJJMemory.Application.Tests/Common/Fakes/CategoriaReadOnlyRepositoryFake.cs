using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Repositories.Categorias;

namespace BJJMemory.Application.Tests.Common.Fakes;

public sealed class CategoriaReadOnlyRepositoryFake : ICategoriaReadOnlyRepository
{
    public Dictionary<Guid, Categoria> Categorias { get; } = [];

    public IList<Categoria> CategoriasByUsuarioIdResult { get; set; } = [];

    public bool ExistCategoriaWithNomeResult { get; set; }

    public bool ExistCategoriaWithNomeExceptIdResult { get; set; }

    public Task<bool> ExistCategoriaWithNome(Guid usuarioId, string nome)
    {
        return Task.FromResult(ExistCategoriaWithNomeResult);
    }

    public Task<bool> ExistCategoriaWithNomeExceptId(Guid usuarioId, string nome, Guid categoriaId)
    {
        return Task.FromResult(ExistCategoriaWithNomeExceptIdResult);
    }

    public Task<IList<Categoria>> GetAllByUsuarioId(Guid usuarioId)
    {
        if (CategoriasByUsuarioIdResult.Count > 0)
        {
            return Task.FromResult(CategoriasByUsuarioIdResult);
        }

        var categorias = Categorias.Values.Where(categoria => categoria.UsuarioId == usuarioId).ToList();
        return Task.FromResult<IList<Categoria>>(categorias);
    }

    public Task<Categoria?> GetById(Guid categoriaId, Guid usuarioId)
    {
        if (Categorias.TryGetValue(categoriaId, out var categoria) && categoria.UsuarioId == usuarioId)
        {
            return Task.FromResult<Categoria?>(categoria);
        }

        return Task.FromResult<Categoria?>(null);
    }
}
