using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Repositories.Posicoes;

namespace BJJMemory.Application.Tests.Common.Fakes;

public sealed class PosicaoWriteOnlyRepositorySpy : IPosicaoWriteOnlyRepository
{
    public Posicao? AddedPosicao { get; private set; }

    public int AddCalls { get; private set; }

    public bool DeleteResult { get; set; } = true;

    public Guid? DeleteReceivedPosicaoId { get; private set; }

    public Guid? DeleteReceivedUsuarioId { get; private set; }

    public Task Add(Posicao posicao)
    {
        AddCalls++;
        AddedPosicao = posicao;
        return Task.CompletedTask;
    }

    public Task<bool> Delete(Guid posicaoId, Guid usuarioId)
    {
        DeleteReceivedPosicaoId = posicaoId;
        DeleteReceivedUsuarioId = usuarioId;
        return Task.FromResult(DeleteResult);
    }
}
