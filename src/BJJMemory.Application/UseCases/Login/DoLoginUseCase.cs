using BJJMemory.Communication.Usuarios.Requests;
using BJJMemory.Communication.Usuarios.Responses;
using BJJMemory.Domain.Repositories.Usuarios;
using BJJMemory.Domain.Security.Cryptography;
using BJJMemory.Domain.Security.Tokens;
using BJJMemory.Exception.ExceptionsBase;

namespace BJJMemory.Application.UseCases.Login;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUsuarioReadOnlyRepository _repository;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public DoLoginUseCase(
        IUsuarioReadOnlyRepository repository,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        _repository = repository;
        _passwordEncripter = passwordEncripter;
        _accessTokenGenerator = accessTokenGenerator;
    }
    public async Task<ResponseRegisterUsuario> Execute(RequestLogin request)
    {
        var user = await _repository.GetUserByEmail(request.Email) ?? throw new InvalidLoginException();

        var passwordMatch = _passwordEncripter.Verify(request.Password, user.HashedPassword);

        if(passwordMatch == false)
        {
            throw new InvalidLoginException();
        }

        return new ResponseRegisterUsuario
        {
            Username = user.Username,
            Token = _accessTokenGenerator.Generate(user)
        };
    }
}

