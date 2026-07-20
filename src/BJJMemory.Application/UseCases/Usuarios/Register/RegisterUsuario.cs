using BJJMemory.Communication.Usuarios.Requests;
using BJJMemory.Communication.Usuarios.Responses;
using BJJMemory.Domain.Entities;
using BJJMemory.Exception;
using BJJMemory.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace BJJMemory.Application.UseCases.Usuarios.Register;

public class RegisterUsuario : IRegisterUsuario
{
    private readonly IUserWriteOnlyRepository _userWriteOnlyrepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUnitOfWork _unityOfWork;
    private readonly IMapper _mapper;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public RegisterUserUseCase(
        IUserWriteOnlyRepository userWriteOnlyrepository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUnitOfWork unityOfWork,
        IMapper mapper,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessToken)
    {
        _userWriteOnlyrepository = userWriteOnlyrepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _unityOfWork = unityOfWork;
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
        _accessTokenGenerator = accessToken;
    }

    public async Task<ResponseRegisterUsuario> Execute(RequestRegisterUsuario request)
    {
        await Validate(request);

        var user = _mapper.Map<Usuario>(request); // Sem automapper, classe privada map to response
        user.PasswordHash = _passwordEncripter.Encrypt(request.Password);
        user.Id = Guid.NewGuid();
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        await _userWriteOnlyrepository.Add(user);

        await _unityOfWork.Commit();

        var token = _accessTokenGenerator.Generate(user);

        return _mapper.Map<ResponseRegisterUsuario>(user, options =>
        {
            options.Items["Token"] = token;
        });
    }

    private async Task Validate(RequestRegisterUsuario request)
    {
        var result = new RegisterUsuarioValidator().Validate(request);

        var emailExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
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
}
