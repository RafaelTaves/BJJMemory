using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Security.Tokens;

namespace BJJMemory.Application.Tests.Common.Fakes;

public sealed class AuthAccessTokenGeneratorFake : IAccessTokenGenerator
{
    public Usuario? LastUsuario { get; private set; }
    public string TokenToReturn { get; set; } = "fake-jwt-token";

    public string Generate(Usuario user)
    {
        LastUsuario = user;
        return TokenToReturn;
    }
}
