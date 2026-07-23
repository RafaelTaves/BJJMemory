using BJJMemory.Domain.Entities;

namespace BJJMemory.Domain.Repositories.Posicoes;

public interface IPosicaoReadOnlyRepository
{
    Task<Posicao?> GetById(Guid posicaoId, Guid usuarioId);

    Task<IList<Posicao>> GetAllByFilters(Guid usuarioId, string? nome, Guid? categoriaId, bool incluirSubcategorias);
}
