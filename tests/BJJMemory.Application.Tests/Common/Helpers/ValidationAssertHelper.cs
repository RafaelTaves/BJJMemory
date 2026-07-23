using BJJMemory.Exception.ExceptionsBase;
using FluentValidation.Results;
using Xunit;

namespace BJJMemory.Application.Tests.Common.Helpers;

public static class ValidationAssertHelper
{
    public static void AssertHasError(ValidationResult validationResult, string expectedErrorMessage)
    {
        Assert.False(validationResult.IsValid);
        Assert.Contains(validationResult.Errors, error => error.ErrorMessage == expectedErrorMessage);
    }

    public static void AssertContainsErrors(ErrorOnValidationException exception, params string[] expectedErrors)
    {
        var errors = exception.GetErrors();

        foreach (var expectedError in expectedErrors)
        {
            Assert.Contains(expectedError, errors);
        }
    }
}
