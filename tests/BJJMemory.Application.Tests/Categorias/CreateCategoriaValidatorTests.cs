using BJJMemory.Application.UseCases.Categorias.Create;
using BJJMemory.Application.Tests.Common.Fakers.Requests;
using BJJMemory.Application.Tests.Common.Helpers;
using BJJMemory.Exception;
using Xunit;

namespace BJJMemory.Application.Tests.Categorias;

public class CreateCategoriaValidatorTests
{
    [Fact]
    public void Should_Be_Valid_When_Request_Is_Valid()
    {
        var validator = new CreateCategoriaValidator();
        var request = RequestCreateCategoriaFaker.Generate();

        var result = validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Should_Return_Error_When_Nome_Is_Empty()
    {
        var validator = new CreateCategoriaValidator();
        var request = RequestCreateCategoriaFaker.Generate(nome: string.Empty);

        var result = validator.Validate(request);

        ValidationAssertHelper.AssertHasError(result, ResourceErrorMessages.CATEGORY_NAME_REQUIRED);
    }

    [Fact]
    public void Should_Return_Error_When_Nome_Length_Is_Invalid()
    {
        var validator = new CreateCategoriaValidator();
        var request = RequestCreateCategoriaFaker.Generate(nome: "a");

        var result = validator.Validate(request);

        ValidationAssertHelper.AssertHasError(result, ResourceErrorMessages.CATEGORY_NAME_LENGTH);
    }
}
