using BJJMemory.Communication.Posicoes.Requests;

namespace BJJMemory.Application.UseCases.Posicoes.Update;

public interface IUpdatePosicao
{
    Task Execute(Guid posicaoId, RequestUpdatePosicao request);
}
