using BJJMemory.Domain.Entities;

namespace BJJMemory.Domain.Repositories.Usuarios;

public interface IUsuarioWriteOnlyRepository
{
    Task Add(Usuario user);

    Task<bool> Delete(Guid id);
}
