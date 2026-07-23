using BJJMemory.Application.UseCases.Categorias.Update;
using BJJMemory.Application.Tests.Common.Fakers.Requests;
using BJJMemory.Application.Tests.Common.Helpers;
using BJJMemory.Exception;
using Xunit;

namespace BJJMemory.Application.Tests.Categorias;

public class UpdateCategoriaValidatorTests
{
    [Fact]
    public void Should_Be_Valid_When_Request_Is_Valid()
    {
        var validator = new UpdateCategoriaValidator();
        var request = RequestUpdateCategoriaFaker.Generate();

        var result = validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Should_Return_Error_When_Nome_Is_Empty()
    {
        var validator = new UpdateCategoriaValidator();
        var request = RequestUpdateCategoriaFaker.Generate(nome: string.Empty);

        var result = validator.Validate(request);

        ValidationAssertHelper.AssertHasError(result, ResourceErrorMessages.CATEGORY_NAME_REQUIRED);
    }

    [Fact]
    public void Should_Return_Error_When_Nome_Length_Is_Invalid()
    {
        var validator = new UpdateCategoriaValidator();
        var request = RequestUpdateCategoriaFaker.Generate(nome: "a");

        var result = validator.Validate(request);

        ValidationAssertHelper.AssertHasError(result, ResourceErrorMessages.CATEGORY_NAME_LENGTH);
    }
}
