using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Repositories.Categorias;

namespace BJJMemory.Application.Tests.Common.Fakes;

public sealed class CategoriaUpdateOnlyRepositorySpy : ICategoriaUpdateOnlyRepository
{
    public int UpdateCalls { get; private set; }

    public Categoria? UpdatedCategoria { get; private set; }

    public void Update(Categoria categoria)
    {
        UpdateCalls++;
        UpdatedCategoria = categoria;
    }
}
