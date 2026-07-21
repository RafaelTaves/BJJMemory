using BJJMemory.Domain.Entities;

namespace BJJMemory.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    Task<Usuario> Get();   
}
