using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Repositories.Usuarios;

namespace BJJMemory.Application.Tests.Common.Fakes;

public sealed class AuthUsuarioWriteOnlyRepositorySpy : IUsuarioWriteOnlyRepository
{
    public Usuario? AddedUsuario { get; private set; }
    public int AddCalls { get; private set; }

    public Task Add(Usuario user)
    {
        AddCalls++;
        AddedUsuario = user;
        return Task.CompletedTask;
    }

    public Task<bool> Delete(Guid id)
    {
        return Task.FromResult(false);
    }
}
