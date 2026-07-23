using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Repositories.Categorias;

namespace BJJMemory.Application.Tests.Common.Fakes;

public sealed class CategoriaWriteOnlyRepositorySpy : ICategoriaWriteOnlyRepository
{
    public List<Categoria> AddedCategorias { get; } = [];

    public bool DeleteResult { get; set; } = true;

    public Guid? ReceivedDeleteCategoriaId { get; private set; }

    public Guid? ReceivedDeleteUsuarioId { get; private set; }

    public Task Add(Categoria categoria)
    {
        AddedCategorias.Add(categoria);
        return Task.CompletedTask;
    }

    public Task<bool> Delete(Guid categoriaId, Guid usuarioId)
    {
        ReceivedDeleteCategoriaId = categoriaId;
        ReceivedDeleteUsuarioId = usuarioId;
        return Task.FromResult(DeleteResult);
    }
}
