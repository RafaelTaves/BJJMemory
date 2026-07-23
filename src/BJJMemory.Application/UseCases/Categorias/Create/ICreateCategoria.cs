using BJJMemory.Communication.Categorias.Requests;
using BJJMemory.Communication.Categorias.Responses;

namespace BJJMemory.Application.UseCases.Categorias.Create;

public interface ICreateCategoria
{
    Task<ResponseCreateCategoria> Execute(RequestCreateCategoria request);
}
