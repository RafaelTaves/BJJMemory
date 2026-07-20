using BJJMemory.Domain.Entities;
namespace BJJMemory.Domain.Repositories.Usuarios;

public interface IUsuarioUpdateOnlyRepository
{
    void Update(Usuario user);
}
