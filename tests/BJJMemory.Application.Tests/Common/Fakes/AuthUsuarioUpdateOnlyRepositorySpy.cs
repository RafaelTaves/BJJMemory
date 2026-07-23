using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Repositories.Usuarios;

namespace BJJMemory.Application.Tests.Common.Fakes;

public sealed class AuthUsuarioUpdateOnlyRepositorySpy : IUsuarioUpdateOnlyRepository
{
    public Usuario? UpdatedUsuario { get; private set; }
    public int UpdateCalls { get; private set; }

    public void Update(Usuario user)
    {
        UpdateCalls++;
        UpdatedUsuario = user;
    }
}
