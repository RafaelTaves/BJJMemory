using BJJMemory.Communication.Categorias.Requests;
using BJJMemory.Exception;
using FluentValidation;

namespace BJJMemory.Application.UseCases.Categorias.Create;

public class CreateCategoriaValidator : AbstractValidator<RequestCreateCategoria>
{
    public CreateCategoriaValidator()
    {
        RuleFor(categoria => categoria.Nome)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.CATEGORY_NAME_REQUIRED)
            .Length(2, 120)
            .WithMessage(ResourceErrorMessages.CATEGORY_NAME_LENGTH);
    }
}
