using BJJMemory.Communication.Usuarios.Requests;
using BJJMemory.Communication.Usuarios.Responses;

namespace BJJMemory.Application.UseCases.Login;

public interface IDoLoginUseCase
{
    Task<ResponseRegisterUsuario> Execute(RequestLogin request);
}
