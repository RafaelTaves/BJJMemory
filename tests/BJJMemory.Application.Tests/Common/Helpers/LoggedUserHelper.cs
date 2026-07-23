using BJJMemory.Application.Tests.Common.Fakers.Entities;
using BJJMemory.Application.Tests.Common.Fakes;
using BJJMemory.Domain.Entities;

namespace BJJMemory.Application.Tests.Common.Helpers;

public static class LoggedUserHelper
{
    public static (Usuario usuario, LoggedUserFake loggedUser) CreateLoggedUser()
    {
        var usuario = UsuarioFaker.Generate();
        return (usuario, new LoggedUserFake(usuario));
    }
}
