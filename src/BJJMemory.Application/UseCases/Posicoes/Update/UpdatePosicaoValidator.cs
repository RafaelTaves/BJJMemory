using BJJMemory.Communication.Posicoes.Requests;
using BJJMemory.Exception;
using FluentValidation;

namespace BJJMemory.Application.UseCases.Posicoes.Update;

public class UpdatePosicaoValidator : AbstractValidator<RequestUpdatePosicao>
{
    public UpdatePosicaoValidator()
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
