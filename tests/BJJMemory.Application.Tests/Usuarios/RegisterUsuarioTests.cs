using BJJMemory.Application.Tests.Common.Fakers.Entities;
using BJJMemory.Application.Tests.Common.Fakers.Requests;
using BJJMemory.Application.Tests.Common.Fakes;
using BJJMemory.Application.Tests.Common.Helpers;
using BJJMemory.Application.UseCases.Usuarios.Register;
using BJJMemory.Exception;
using BJJMemory.Exception.ExceptionsBase;
using Xunit;

namespace BJJMemory.Application.Tests.Usuarios;

public class RegisterUsuarioTests
{
    [Fact]
    public async Task Should_Register_Usuario_With_Encrypted_Password_And_Return_Token()
    {
        var writeRepository = new AuthUsuarioWriteOnlyRepositorySpy();
        var readOnlyRepository = new AuthUsuarioReadOnlyRepositoryFake();
        var unitOfWork = new SpyUnitOfWork();
        var passwordEncripter = new AuthPasswordEncripterFake();
        var tokenGenerator = new AuthAccessTokenGeneratorFake { TokenToReturn = "token-register-sucesso" };
        var useCase = CreateUseCase(writeRepository, readOnlyRepository, unitOfWork, passwordEncripter, tokenGenerator);
        var request = RequestRegisterUsuarioFaker.Generate(
            username: "usuario.teste",
            email: "usuario@bjjmemory.com",
            password: "Password@123");

        var response = await useCase.Execute(request);

        Assert.Equal("usuario.teste", response.Username);
        Assert.Equal("token-register-sucesso", response.Token);
        Assert.Equal(1, writeRepository.AddCalls);
        Assert.Equal(1, unitOfWork.CommitCalls);
        Assert.NotNull(writeRepository.AddedUsuario);
        Assert.Equal(request.Username, writeRepository.AddedUsuario!.Username);
        Assert.Equal(request.Email, writeRepository.AddedUsuario.Email);
        Assert.Equal(passwordEncripter.Encrypt(request.Password), writeRepository.AddedUsuario.HashedPassword);
        Assert.NotEqual(request.Password, writeRepository.AddedUsuario.HashedPassword);
        Assert.Same(writeRepository.AddedUsuario, tokenGenerator.LastUsuario);
    }

    [Fact]
    public async Task Should_Throw_ErrorOnValidationException_When_Email_Already_Exists()
    {
        var writeRepository = new AuthUsuarioWriteOnlyRepositorySpy();
        var readOnlyRepository = new AuthUsuarioReadOnlyRepositoryFake();
        var existentUser = UsuarioFaker.Generate(email: "usuario@bjjmemory.com");
        readOnlyRepository.AddUser(existentUser);
        var useCase = CreateUseCase(
            writeRepository,
            readOnlyRepository,
            new SpyUnitOfWork(),
            new AuthPasswordEncripterFake(),
            new AuthAccessTokenGeneratorFake());
        var request = RequestRegisterUsuarioFaker.Generate(email: existentUser.Email);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => useCase.Execute(request));

        ValidationAssertHelper.AssertContainsErrors(exception, ResourceErrorMessages.EMAIL_ALREADY_EXISTS);
        Assert.Equal(0, writeRepository.AddCalls);
    }

    [Fact]
    public async Task Should_Throw_ErrorOnValidationException_When_Request_Is_Invalid()
    {
        var writeRepository = new AuthUsuarioWriteOnlyRepositorySpy();
        var useCase = CreateUseCase(
            writeRepository,
            new AuthUsuarioReadOnlyRepositoryFake(),
            new SpyUnitOfWork(),
            new AuthPasswordEncripterFake(),
            new AuthAccessTokenGeneratorFake());
        var request = RequestRegisterUsuarioFaker.Generate(
            username: string.Empty,
            email: "email-invalido",
            password: string.Empty);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => useCase.Execute(request));
        ValidationAssertHelper.AssertContainsErrors(
            exception,
            ResourceErrorMessages.USER_NAME_REQUIRED,
            ResourceErrorMessages.EMAIL_INVALID,
            ResourceErrorMessages.PASSWORD_REQUIRED);
        Assert.Equal(0, writeRepository.AddCalls);
    }

    private static RegisterUsuario CreateUseCase(
        AuthUsuarioWriteOnlyRepositorySpy writeRepository,
        AuthUsuarioReadOnlyRepositoryFake readOnlyRepository,
        SpyUnitOfWork unitOfWork,
        AuthPasswordEncripterFake passwordEncripter,
        AuthAccessTokenGeneratorFake tokenGenerator)
    {
        return new RegisterUsuario(
            writeRepository,
            readOnlyRepository,
            unitOfWork,
            passwordEncripter,
            tokenGenerator,
            new RegisterUsuarioValidator());
    }
}
