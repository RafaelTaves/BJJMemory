using BJJMemory.Domain.Entities;

namespace BJJMemory.Domain.Repositories.Usuarios;

public interface IUsuarioReadOnlyRepository
{
    Task<bool> ExistUserWithEmail(string email);

    Task<Usuario> GetUserByEmail(string email);
}
