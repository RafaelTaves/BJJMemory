using BJJMemory.Communication.Categorias.Requests;
using BJJMemory.Exception;
using FluentValidation;

namespace BJJMemory.Application.UseCases.Categorias.Update;

public class UpdateCategoriaValidator : AbstractValidator<RequestUpdateCategoria>
{
    public UpdateCategoriaValidator()
    {
        RuleFor(categoria => categoria.Nome)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.CATEGORY_NAME_REQUIRED)
            .Length(2, 120)
            .WithMessage(ResourceErrorMessages.CATEGORY_NAME_LENGTH);
    }
}
