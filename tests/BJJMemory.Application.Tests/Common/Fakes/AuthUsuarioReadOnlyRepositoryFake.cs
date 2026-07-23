using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Repositories.Usuarios;

namespace BJJMemory.Application.Tests.Common.Fakes;

public sealed class AuthUsuarioReadOnlyRepositoryFake : IUsuarioReadOnlyRepository
{
    private readonly Dictionary<string, Usuario> _usersByEmail = new(StringComparer.OrdinalIgnoreCase);

    public void AddUser(Usuario user)
    {
        _usersByEmail[user.Email] = user;
    }

    public Task<bool> ExistUserWithEmail(string email)
    {
        return Task.FromResult(_usersByEmail.ContainsKey(email));
    }

    public Task<Usuario?> GetUserByEmail(string email)
    {
        _usersByEmail.TryGetValue(email, out var user);
        return Task.FromResult(user);
    }
}
