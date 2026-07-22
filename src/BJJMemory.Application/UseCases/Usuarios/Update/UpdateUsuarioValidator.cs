using BJJMemory.Communication.Usuarios.Requests;
using BJJMemory.Exception;
using FluentValidation;

namespace BJJMemory.Application.UseCases.Usuarios.Update;

public class UpdateUsuarioValidator : AbstractValidator<RequestUpdateUsuario>
{
    public UpdateUsuarioValidator()
    {
        RuleFor(user => user.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.USER_NAME_REQUIRED)
            .Length(2, 120)
            .WithMessage(ResourceErrorMessages.USER_NAME_LENGTH);

        RuleFor(user => user.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMAIL_REQUIRED)
            .EmailAddress()
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);
    }
}
