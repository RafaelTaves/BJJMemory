using BJJMemory.Application.Tests.Common.Fakers.Entities;
using BJJMemory.Application.Tests.Common.Fakes;
using BJJMemory.Application.UseCases.Usuarios.Get;
using Xunit;

namespace BJJMemory.Application.Tests.Usuarios;

public class GetUsuarioTests
{
    [Fact]
    public async Task Should_Map_Usuario_To_Response_Correctly()
    {
        var user = UsuarioFaker.Generate(username: "usuario.mapeado", email: "mapeado@email.com");
        var useCase = new GetUsuario(new LoggedUserFake(user));

        var response = await useCase.Execute();

        Assert.Equal(user.Id, response.Id);
        Assert.Equal(user.Username, response.Username);
        Assert.Equal(user.Email, response.Email);
    }
}
