using BJJMemory.Communication.Usuarios.Requests;
using BJJMemory.Communication.Usuarios.Responses;
using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Repositories;
using BJJMemory.Domain.Repositories.Usuarios;
using BJJMemory.Domain.Security.Cryptography;
using BJJMemory.Domain.Security.Tokens;
using BJJMemory.Exception;
using BJJMemory.Exception.ExceptionsBase;
using FluentValidation;
using FluentValidation.Results;

namespace BJJMemory.Application.UseCases.Usuarios.Register;

public class RegisterUsuario : IRegisterUsuario
{
    private readonly IUsuarioWriteOnlyRepository _usuarioWriteOnlyRepository;
    private readonly IUsuarioReadOnlyRepository _usuarioReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IValidator<RequestRegisterUsuario> _validator;

    public RegisterUsuario(
        IUsuarioWriteOnlyRepository userWriteOnlyRepository,
        IUsuarioReadOnlyRepository userReadOnlyRepository,
        IUnitOfWork unitOfWork,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessToken,
        IValidator<RequestRegisterUsuario> validator)
    {
        _usuarioWriteOnlyRepository = userWriteOnlyRepository;
        _usuarioReadOnlyRepository = userReadOnlyRepository;
        _unitOfWork = unitOfWork;
        _passwordEncripter = passwordEncripter;
        _accessTokenGenerator = accessToken;
        _validator = validator;
    }

    public async Task<ResponseRegisterUsuario> Execute(RequestRegisterUsuario request)
    {
        await Validate(request);

        var user = MapToEntity(request);

        await _usuarioWriteOnlyRepository.Add(user);

        await _unitOfWork.Commit();

        var token = _accessTokenGenerator.Generate(user);

        return new ResponseRegisterUsuario
        {
            Username = user.Username,
            Token = token,
        };
    }

    private async Task Validate(RequestRegisterUsuario request)
    {
        var result = await _validator.ValidateAsync(request);

        var emailExist = await _usuarioReadOnlyRepository.ExistUserWithEmail(request.Email);
        if (emailExist)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_EXISTS));
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }

    private Usuario MapToEntity(RequestRegisterUsuario request)
    {
        var user = Usuario.Create(request.Username, request.Email, _passwordEncripter.Encrypt(request.Password));
        
        return user;
    }
}
