using BJJMemory.Communication.Usuarios.Responses;
using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Services.LoggedUser;

namespace BJJMemory.Application.UseCases.Usuarios.Get;

public class GetUsuario : IGetUsuario
{
    private readonly ILoggedUser _loggedUser;

    public GetUsuario(ILoggedUser loggedUser)
    {
        _loggedUser = loggedUser;
    }
    public async Task<ResponseGetUsuario> Execute()
    {
        var user = await _loggedUser.Get();

        return MapToResponse(user);
    }

    private static ResponseGetUsuario MapToResponse(Usuario user)
    {
        return new ResponseGetUsuario
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email
        };
    }
}
