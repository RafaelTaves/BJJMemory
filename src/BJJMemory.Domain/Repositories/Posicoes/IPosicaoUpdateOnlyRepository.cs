using BJJMemory.Domain.Entities;

namespace BJJMemory.Domain.Repositories.Posicoes;

public interface IPosicaoUpdateOnlyRepository
{
    void Update(Posicao posicao);
}
