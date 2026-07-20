using BJJMemory.Communication.Usuarios.Requests;
using BJJMemory.Communication.Usuarios.Responses;

namespace BJJMemory.Application.UseCases.Usuarios.Register;

public interface IRegisterUsuario
{
    Task<RegisterUsuarioResponse> Execute(RequestRegisterUsuario request);
}
