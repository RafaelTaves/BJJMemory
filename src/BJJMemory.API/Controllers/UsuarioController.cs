using BJJMemory.Application.UseCases.Usuarios.Register;
using BJJMemory.Communication.Responses;
using BJJMemory.Communication.Usuarios.Requests;
using BJJMemory.Communication.Usuarios.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BJJMemory.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsuarioController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUsuario), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUsuario useCase,
        [FromBody] RequestRegisterUsuario request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }
}
