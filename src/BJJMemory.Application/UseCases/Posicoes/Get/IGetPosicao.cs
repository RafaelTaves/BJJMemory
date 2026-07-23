using BJJMemory.Communication.Posicoes.Requests;
using BJJMemory.Communication.Posicoes.Responses;

namespace BJJMemory.Application.UseCases.Posicoes.Get;

public interface IGetPosicao
{
    Task<IList<ResponseGetPosicao>> Execute(RequestGetPosicaoFilter request);
}
