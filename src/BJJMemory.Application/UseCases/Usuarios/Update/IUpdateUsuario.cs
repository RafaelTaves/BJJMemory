using BJJMemory.Communication.Usuarios.Requests;

namespace BJJMemory.Application.UseCases.Usuarios.Update;

public interface IUpdateUsuario
{
    Task Execute(RequestUpdateUsuario request);
}
