using BJJMemory.Application.Tests.Common.Fakers.Entities;
using BJJMemory.Application.Tests.Common.Fakers.Requests;
using BJJMemory.Application.Tests.Common.Fakes;
using BJJMemory.Application.UseCases.Login;
using BJJMemory.Exception.ExceptionsBase;
using Xunit;

namespace BJJMemory.Application.Tests.Login;

public class DoLoginUseCaseTests
{
    [Fact]
    public async Task Should_Do_Login_When_Credentials_Are_Valid()
    {
        var passwordEncripter = new AuthPasswordEncripterFake();
        var tokenGenerator = new AuthAccessTokenGeneratorFake { TokenToReturn = "token-login-sucesso" };
        var repository = new AuthUsuarioReadOnlyRepositoryFake();
        var password = "Password@123";
        var user = UsuarioFaker.Generate(email: "login@bjjmemory.com", hashedPassword: passwordEncripter.Encrypt(password));
        repository.AddUser(user);
        var request = RequestLoginFaker.Generate(email: user.Email, password: password);
        var useCase = new DoLoginUseCase(repository, passwordEncripter, tokenGenerator);

        var result = await useCase.Execute(request);

        Assert.Equal(user.Username, result.Username);
        Assert.Equal("token-login-sucesso", result.Token);
    }

    [Fact]
    public async Task Should_Throw_InvalidLoginException_When_User_Does_Not_Exist()
    {
        var useCase = new DoLoginUseCase(
            new AuthUsuarioReadOnlyRepositoryFake(),
            new AuthPasswordEncripterFake(),
            new AuthAccessTokenGeneratorFake());
        var request = RequestLoginFaker.Generate();

        await Assert.ThrowsAsync<InvalidLoginException>(() => useCase.Execute(request));
    }

    [Fact]
    public async Task Should_Throw_InvalidLoginException_When_Password_Is_Invalid()
    {
        var passwordEncripter = new AuthPasswordEncripterFake();
        var repository = new AuthUsuarioReadOnlyRepositoryFake();
        var user = UsuarioFaker.Generate(email: "login@bjjmemory.com", hashedPassword: passwordEncripter.Encrypt("Password@123"));
        repository.AddUser(user);
        var request = RequestLoginFaker.Generate(email: user.Email, password: "WrongPassword@123");
        var useCase = new DoLoginUseCase(repository, passwordEncripter, new AuthAccessTokenGeneratorFake());

        await Assert.ThrowsAsync<InvalidLoginException>(() => useCase.Execute(request));
    }
}
