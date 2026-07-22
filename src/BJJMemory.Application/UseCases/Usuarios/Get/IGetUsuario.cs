using BJJMemory.Communication.Usuarios.Responses;

namespace BJJMemory.Application.UseCases.Usuarios.Get;

public interface IGetUsuario
{
    Task<ResponseGetUsuario> Execute();
}
