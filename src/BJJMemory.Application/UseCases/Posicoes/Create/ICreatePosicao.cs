using BJJMemory.Communication.Posicoes.Requests;
using BJJMemory.Communication.Posicoes.Responses;

namespace BJJMemory.Application.UseCases.Posicoes.Create;

public interface ICreatePosicao
{
    Task<ResponseCreatePosicao> Execute(RequestCreatePosicao request);
}
