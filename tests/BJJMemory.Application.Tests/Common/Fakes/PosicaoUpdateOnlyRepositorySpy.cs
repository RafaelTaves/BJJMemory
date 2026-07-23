using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Repositories.Posicoes;

namespace BJJMemory.Application.Tests.Common.Fakes;

public sealed class PosicaoUpdateOnlyRepositorySpy : IPosicaoUpdateOnlyRepository
{
    public Posicao? UpdatedPosicao { get; private set; }

    public int UpdateCalls { get; private set; }

    public void Update(Posicao posicao)
    {
        UpdateCalls++;
        UpdatedPosicao = posicao;
    }
}
