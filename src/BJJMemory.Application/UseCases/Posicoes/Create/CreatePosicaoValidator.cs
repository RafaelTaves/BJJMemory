using BJJMemory.Communication.Posicoes.Requests;
using BJJMemory.Exception;
using FluentValidation;

namespace BJJMemory.Application.UseCases.Posicoes.Create;

public class CreatePosicaoValidator : AbstractValidator<RequestCreatePosicao>
{
    public CreatePosicaoValidator()
    {
        RuleFor(posicao => posicao.CategoriaId)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.POSITION_CATEGORY_REQUIRED);

        RuleFor(posicao => posicao.Titulo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.POSITION_TITLE_REQUIRED);

        RuleFor(posicao => posicao.Descricao)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.POSITION_DESCRIPTION_REQUIRED);
    }
}
