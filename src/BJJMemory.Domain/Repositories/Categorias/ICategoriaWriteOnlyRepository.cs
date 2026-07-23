using BJJMemory.Domain.Entities;

namespace BJJMemory.Domain.Repositories.Categorias;

public interface ICategoriaWriteOnlyRepository
{
    Task Add(Categoria categoria);

    Task<bool> Delete(Guid categoriaId, Guid usuarioId);
}
