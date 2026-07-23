using BJJMemory.Application.UseCases.Posicoes.Update;
using BJJMemory.Application.Tests.Common.Fakers.Requests;
using BJJMemory.Application.Tests.Common.Helpers;
using BJJMemory.Exception;
using Xunit;

namespace BJJMemory.Application.Tests.Posicoes;

public class UpdatePosicaoValidatorTests
{
    [Fact]
    public void Should_Be_Valid_When_Request_Is_Valid()
    {
        var validator = new UpdatePosicaoValidator();
        var request = RequestUpdatePosicaoFaker.Generate();

        var result = validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Should_Return_Error_When_CategoriaId_Is_Empty()
    {
        var validator = new UpdatePosicaoValidator();
        var request = RequestUpdatePosicaoFaker.Generate(categoriaId: Guid.Empty);

        var result = validator.Validate(request);

        ValidationAssertHelper.AssertHasError(result, ResourceErrorMessages.POSITION_CATEGORY_REQUIRED);
    }

    [Fact]
    public void Should_Return_Error_When_Titulo_Is_Empty()
    {
        var validator = new UpdatePosicaoValidator();
        var request = RequestUpdatePosicaoFaker.Generate(titulo: string.Empty);

        var result = validator.Validate(request);

        ValidationAssertHelper.AssertHasError(result, ResourceErrorMessages.POSITION_TITLE_REQUIRED);
    }

    [Fact]
    public void Should_Return_Error_When_Descricao_Is_Empty()
    {
        var validator = new UpdatePosicaoValidator();
        var request = RequestUpdatePosicaoFaker.Generate(descricao: string.Empty);

        var result = validator.Validate(request);

        ValidationAssertHelper.AssertHasError(result, ResourceErrorMessages.POSITION_DESCRIPTION_REQUIRED);
    }
}
