using BJJMemory.Application.UseCases.Login;
using BJJMemory.Communication.Responses;
using BJJMemory.Communication.Usuarios.Requests;
using BJJMemory.Communication.Usuarios.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BJJMemory.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUsuario), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromServices] IDoLoginUseCase useCase,
        [FromBody] RequestLogin request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }
}
