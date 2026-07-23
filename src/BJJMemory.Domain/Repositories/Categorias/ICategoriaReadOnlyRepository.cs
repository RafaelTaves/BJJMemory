using BJJMemory.Domain.Entities;

namespace BJJMemory.Domain.Repositories.Categorias;

public interface ICategoriaReadOnlyRepository
{
    Task<bool> ExistCategoriaWithNome(Guid usuarioId, string nome);

    Task<bool> ExistCategoriaWithNomeExceptId(Guid usuarioId, string nome, Guid categoriaId);

    Task<Categoria?> GetById(Guid categoriaId, Guid usuarioId);

    Task<IList<Categoria>> GetAllByUsuarioId(Guid usuarioId);
}
