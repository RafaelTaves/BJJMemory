using BJJMemory.Application.Tests.Common.Fakers.Entities;
using BJJMemory.Application.Tests.Common.Fakers.Requests;
using BJJMemory.Application.Tests.Common.Fakes;
using BJJMemory.Application.Tests.Common.Helpers;
using BJJMemory.Application.UseCases.Usuarios.Update;
using BJJMemory.Exception;
using BJJMemory.Exception.ExceptionsBase;
using Xunit;

namespace BJJMemory.Application.Tests.Usuarios;

public class UpdateUsuarioTests
{
    [Fact]
    public async Task Should_Update_Logged_User_And_Commit()
    {
        var (loggedUsuario, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var updateRepository = new AuthUsuarioUpdateOnlyRepositorySpy();
        var unitOfWork = new SpyUnitOfWork();
        var request = RequestUpdateUsuarioFaker.Generate(username: "novo.username", email: "novo@email.com");
        var useCase = new UpdateUsuario(
            loggedUser,
            updateRepository,
            new AuthUsuarioReadOnlyRepositoryFake(),
            unitOfWork);
        var oldHashedPassword = loggedUsuario.HashedPassword;

        await useCase.Execute(request);

        Assert.Equal(1, updateRepository.UpdateCalls);
        Assert.Equal(1, unitOfWork.CommitCalls);
        Assert.NotNull(updateRepository.UpdatedUsuario);
        Assert.Same(loggedUsuario, updateRepository.UpdatedUsuario);
        Assert.Equal(request.Username, loggedUsuario.Username);
        Assert.Equal(request.Email, loggedUsuario.Email);
        Assert.Equal(oldHashedPassword, loggedUsuario.HashedPassword);
    }

    [Fact]
    public async Task Should_Throw_ErrorOnValidationException_When_Email_Is_Used_By_Another_User()
    {
        var (loggedUsuario, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var readOnlyRepository = new AuthUsuarioReadOnlyRepositoryFake();
        var anotherUser = UsuarioFaker.Generate(email: "duplicado@email.com");
        readOnlyRepository.AddUser(anotherUser);
        var useCase = new UpdateUsuario(
            loggedUser,
            new AuthUsuarioUpdateOnlyRepositorySpy(),
            readOnlyRepository,
            new SpyUnitOfWork());
        var request = RequestUpdateUsuarioFaker.Generate(username: "novo.username", email: anotherUser.Email);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => useCase.Execute(request));

        ValidationAssertHelper.AssertContainsErrors(exception, ResourceErrorMessages.EMAIL_ALREADY_EXISTS);
        Assert.NotEqual(loggedUsuario.Id, anotherUser.Id);
    }

    [Fact]
    public async Task Should_Throw_ErrorOnValidationException_When_Request_Is_Invalid()
    {
        var (_, loggedUser) = LoggedUserHelper.CreateLoggedUser();
        var updateRepository = new AuthUsuarioUpdateOnlyRepositorySpy();
        var unitOfWork = new SpyUnitOfWork();
        var useCase = new UpdateUsuario(
            loggedUser,
            updateRepository,
            new AuthUsuarioReadOnlyRepositoryFake(),
            unitOfWork);
        var request = RequestUpdateUsuarioFaker.Generate(username: string.Empty, email: "email-invalido");

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => useCase.Execute(request));
        ValidationAssertHelper.AssertContainsErrors(
            exception,
            ResourceErrorMessages.USER_NAME_REQUIRED,
            ResourceErrorMessages.EMAIL_INVALID);
        Assert.Equal(0, updateRepository.UpdateCalls);
        Assert.Equal(0, unitOfWork.CommitCalls);
    }
}
