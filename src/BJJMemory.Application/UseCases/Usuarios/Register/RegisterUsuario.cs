using BJJMemory.Communication.Usuarios.Requests;
using BJJMemory.Communication.Usuarios.Responses;
using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Repositories;
using BJJMemory.Domain.Repositories.Usuarios;
using BJJMemory.Domain.Security.Cryptography;
using BJJMemory.Domain.Security.Tokens;
using BJJMemory.Exception;
using BJJMemory.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace BJJMemory.Application.UseCases.Usuarios.Register;

public class RegisterUsuario : IRegisterUsuario
{
    private readonly IUsuarioWriteOnlyRepository _userWriteOnlyrepository;
    private readonly IUsuarioReadOnlyRepository _userReadOnlyRepository;
    private readonly IUnitOfWork _unityOfWork;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public RegisterUsuario(
        IUsuarioWriteOnlyRepository userWriteOnlyrepository,
        IUsuarioReadOnlyRepository userReadOnlyRepository,
        IUnitOfWork unityOfWork,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessToken)
    {
        _userWriteOnlyrepository = userWriteOnlyrepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _unityOfWork = unityOfWork;
        _passwordEncripter = passwordEncripter;
        _accessTokenGenerator = accessToken;
    }

    public async Task<ResponseRegisterUsuario> Execute(RequestRegisterUsuario request)
    {
        await Validate(request);

        var user = MapToResponse(request);

        await _userWriteOnlyrepository.Add(user);

        await _unityOfWork.Commit();

        var token = _accessTokenGenerator.Generate(user);

        return new ResponseRegisterUsuario
        {
            Username = user.Username,
            Token = token,
        };
    }

    private async Task Validate(RequestRegisterUsuario request)
    {
        var result = new RegisterUsuarioValidator().Validate(request);

        var emailExist = await _userReadOnlyRepository.ExistUserWithEmail(request.Email);
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

    private Usuario MapToResponse(RequestRegisterUsuario request)
    {
        var user =  Usuario.Create(request.Username, request.Email, _passwordEncripter.Encrypt(request.Password));
        
        return user;
    }
}
