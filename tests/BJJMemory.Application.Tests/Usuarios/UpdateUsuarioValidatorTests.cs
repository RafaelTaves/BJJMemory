using BJJMemory.Application.UseCases.Usuarios.Update;
using BJJMemory.Application.Tests.Common.Fakers.Requests;
using BJJMemory.Application.Tests.Common.Helpers;
using BJJMemory.Exception;
using Xunit;

namespace BJJMemory.Application.Tests.Usuarios;

public class UpdateUsuarioValidatorTests
{
    [Fact]
    public void Should_Be_Valid_When_Request_Is_Valid()
    {
        var validator = new UpdateUsuarioValidator();
        var request = RequestUpdateUsuarioFaker.Generate();

        var result = validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Should_Return_Error_When_Username_Is_Empty()
    {
        var validator = new UpdateUsuarioValidator();
        var request = RequestUpdateUsuarioFaker.Generate(username: string.Empty);

        var result = validator.Validate(request);

        ValidationAssertHelper.AssertHasError(result, ResourceErrorMessages.USER_NAME_REQUIRED);
    }

    [Fact]
    public void Should_Return_Error_When_Username_Length_Is_Invalid()
    {
        var validator = new UpdateUsuarioValidator();
        var request = RequestUpdateUsuarioFaker.Generate(username: "a");

        var result = validator.Validate(request);

        ValidationAssertHelper.AssertHasError(result, ResourceErrorMessages.USER_NAME_LENGTH);
    }

    [Fact]
    public void Should_Return_Error_When_Email_Is_Empty()
    {
        var validator = new UpdateUsuarioValidator();
        var request = RequestUpdateUsuarioFaker.Generate(email: string.Empty);

        var result = validator.Validate(request);

        ValidationAssertHelper.AssertHasError(result, ResourceErrorMessages.EMAIL_REQUIRED);
    }

    [Fact]
    public void Should_Return_Error_When_Email_Is_Invalid()
    {
        var validator = new UpdateUsuarioValidator();
        var request = RequestUpdateUsuarioFaker.Generate(email: "email-invalido");

        var result = validator.Validate(request);

        ValidationAssertHelper.AssertHasError(result, ResourceErrorMessages.EMAIL_INVALID);
    }
}
