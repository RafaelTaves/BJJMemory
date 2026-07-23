using BJJMemory.Application.UseCases.Usuarios.Register;
using BJJMemory.Application.Tests.Common.Fakers.Requests;
using BJJMemory.Application.Tests.Common.Helpers;
using BJJMemory.Exception;
using Xunit;

namespace BJJMemory.Application.Tests.Usuarios;

public class RegisterUsuarioValidatorTests
{
    private const string ValidPassword = "Abcd12@";

    [Fact]
    public void Should_Be_Valid_When_Request_Is_Valid()
    {
        var validator = new RegisterUsuarioValidator();
        var request = RequestRegisterUsuarioFaker.Generate(password: ValidPassword);

        var result = validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Should_Return_Error_When_Username_Is_Empty()
    {
        var validator = new RegisterUsuarioValidator();
        var request = RequestRegisterUsuarioFaker.Generate(username: string.Empty, password: ValidPassword);

        var result = validator.Validate(request);

        ValidationAssertHelper.AssertHasError(result, ResourceErrorMessages.USER_NAME_REQUIRED);
    }

    [Fact]
    public void Should_Return_Error_When_Username_Length_Is_Invalid()
    {
        var validator = new RegisterUsuarioValidator();
        var request = RequestRegisterUsuarioFaker.Generate(username: "a", password: ValidPassword);

        var result = validator.Validate(request);

        ValidationAssertHelper.AssertHasError(result, ResourceErrorMessages.USER_NAME_LENGTH);
    }

    [Fact]
    public void Should_Return_Error_When_Email_Is_Empty()
    {
        var validator = new RegisterUsuarioValidator();
        var request = RequestRegisterUsuarioFaker.Generate(email: string.Empty, password: ValidPassword);

        var result = validator.Validate(request);

        ValidationAssertHelper.AssertHasError(result, ResourceErrorMessages.EMAIL_REQUIRED);
    }

    [Fact]
    public void Should_Return_Error_When_Email_Is_Invalid()
    {
        var validator = new RegisterUsuarioValidator();
        var request = RequestRegisterUsuarioFaker.Generate(email: "email-invalido", password: ValidPassword);

        var result = validator.Validate(request);

        ValidationAssertHelper.AssertHasError(result, ResourceErrorMessages.EMAIL_INVALID);
    }

    [Fact]
    public void Should_Return_Error_When_Password_Is_Empty()
    {
        var validator = new RegisterUsuarioValidator();
        var request = RequestRegisterUsuarioFaker.Generate(password: string.Empty);

        var result = validator.Validate(request);

        ValidationAssertHelper.AssertHasError(result, ResourceErrorMessages.PASSWORD_REQUIRED);
    }

    [Fact]
    public void Should_Return_Error_When_Password_Is_Too_Short()
    {
        var validator = new RegisterUsuarioValidator();
        var request = RequestRegisterUsuarioFaker.Generate(password: "Ab1@a");

        var result = validator.Validate(request);

        ValidationAssertHelper.AssertHasError(result, ResourceErrorMessages.PASSWORD_MIN_LENGTH);
    }

    [Fact]
    public void Should_Return_Error_When_Password_Has_No_Uppercase()
    {
        var validator = new RegisterUsuarioValidator();
        var request = RequestRegisterUsuarioFaker.Generate(password: "abcd12@");

        var result = validator.Validate(request);

        ValidationAssertHelper.AssertHasError(result, ResourceErrorMessages.PASSWORD_UPPERCASE_LETTER);
    }

    [Fact]
    public void Should_Return_Error_When_Password_Has_No_Lowercase()
    {
        var validator = new RegisterUsuarioValidator();
        var request = RequestRegisterUsuarioFaker.Generate(password: "ABCD12@");

        var result = validator.Validate(request);

        ValidationAssertHelper.AssertHasError(result, ResourceErrorMessages.PASSWORD_LOWERCASE_LETTER);
    }

    [Fact]
    public void Should_Return_Error_When_Password_Has_No_Number()
    {
        var validator = new RegisterUsuarioValidator();
        var request = RequestRegisterUsuarioFaker.Generate(password: "AbcdEf@");

        var result = validator.Validate(request);

        ValidationAssertHelper.AssertHasError(result, ResourceErrorMessages.PASSWORD_NUMBER);
    }

    [Fact]
    public void Should_Return_Error_When_Password_Has_No_Special_Character()
    {
        var validator = new RegisterUsuarioValidator();
        var request = RequestRegisterUsuarioFaker.Generate(password: "Abcd1234");

        var result = validator.Validate(request);

        ValidationAssertHelper.AssertHasError(result, ResourceErrorMessages.PASSWORD_SPECIAL_CHARACTER);
    }
}
