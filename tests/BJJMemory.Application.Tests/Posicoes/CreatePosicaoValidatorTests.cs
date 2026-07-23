using BJJMemory.Application.UseCases.Posicoes.Create;
using BJJMemory.Communication.Posicoes.Requests;
using BJJMemory.Exception;
using Xunit;

namespace BJJMemory.Application.Tests.Posicoes;

public class CreatePosicaoValidatorTests
{
    [Fact]
    public void Should_Return_Error_When_Titulo_Is_Empty()
    {
        var validator = new CreatePosicaoValidator();
        var request = new RequestCreatePosicao
        {
            CategoriaId = Guid.NewGuid(),
            Titulo = string.Empty,
            Descricao = "Descricao da posicao"
        };

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.ErrorMessage == ResourceErrorMessages.POSITION_TITLE_REQUIRED);
    }

    [Fact]
    public void Should_Return_Error_When_Descricao_Is_Empty()
    {
        var validator = new CreatePosicaoValidator();
        var request = new RequestCreatePosicao
        {
            CategoriaId = Guid.NewGuid(),
            Titulo = "Arm lock",
            Descricao = string.Empty
        };

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.ErrorMessage == ResourceErrorMessages.POSITION_DESCRIPTION_REQUIRED);
    }
}
