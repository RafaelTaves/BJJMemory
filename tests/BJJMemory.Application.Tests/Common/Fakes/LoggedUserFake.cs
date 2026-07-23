using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Services.LoggedUser;

namespace BJJMemory.Application.Tests.Common.Fakes;

public sealed class LoggedUserFake : ILoggedUser
{
    private readonly Usuario _usuario;

    public LoggedUserFake(Usuario usuario)
    {
        _usuario = usuario;
    }

    public Task<Usuario> Get()
    {
        return Task.FromResult(_usuario);
    }
}
