using BJJMemory.Domain.Entities;

namespace BJJMemory.Domain.Repositories.Categorias;

public interface ICategoriaUpdateOnlyRepository
{
    void Update(Categoria categoria);
}
