namespace BJJMemory.Application.UseCases.Usuarios.Update;

using BJJMemory.Communication.Usuarios.Requests;
using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Repositories;
using BJJMemory.Domain.Repositories.Usuarios;
using BJJMemory.Domain.Services.LoggedUser;
using BJJMemory.Exception;
using BJJMemory.Exception.ExceptionsBase;
using FluentValidation.Results;

public class UpdateUsuario : IUpdateUsuario
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUsuarioUpdateOnlyRepository _usuarioUpdateOnlyRepository;
    private readonly IUsuarioReadOnlyRepository _usuarioReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUsuario(
        ILoggedUser loggedUser,
        IUsuarioUpdateOnlyRepository usuarioUpdateOnlyRepository,
        IUsuarioReadOnlyRepository usuarioReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _usuarioUpdateOnlyRepository = usuarioUpdateOnlyRepository;
        _usuarioReadOnlyRepository = usuarioReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestUpdateUsuario request)
    {
        var loggedUser = await _loggedUser.Get();

        await Validate(request, loggedUser.Id);

        var user = MapToUsuario(request, loggedUser);

        _usuarioUpdateOnlyRepository.Update(user);
        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUsuario request, Guid loggedUserId)
    {
        var result = new UpdateUsuarioValidator().Validate(request);

        var userWithEmail = await _usuarioReadOnlyRepository.GetUserByEmail(request.Email);
        if (userWithEmail is not null && userWithEmail.Id != loggedUserId)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_EXISTS));
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }

    private static Usuario MapToUsuario(RequestUpdateUsuario request, Usuario loggedUser)
    {
        loggedUser.Update(request.Username, request.Email, loggedUser.HashedPassword);

        return loggedUser;
    }
}
