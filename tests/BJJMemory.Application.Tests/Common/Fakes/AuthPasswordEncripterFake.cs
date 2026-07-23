using BJJMemory.Domain.Security.Cryptography;

namespace BJJMemory.Application.Tests.Common.Fakes;

public sealed class AuthPasswordEncripterFake : IPasswordEncripter
{
    private const string Prefix = "hashed::";

    public string Encrypt(string password)
    {
        return $"{Prefix}{password}";
    }

    public bool Verify(string password, string passwordHash)
    {
        return Encrypt(password) == passwordHash;
    }
}
