using BJJMemory.Domain.Entities;

namespace BJJMemory.Domain.Repositories.Posicoes;

public interface IPosicaoWriteOnlyRepository
{
    Task Add(Posicao posicao);

    Task<bool> Delete(Guid posicaoId, Guid usuarioId);
}
