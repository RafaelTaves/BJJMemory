using BJJMemory.Domain.Entities;

namespace BJJMemory.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    string Generate(Usuario user);
}
