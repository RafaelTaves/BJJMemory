using BJJMemory.Application.UseCases.Posicoes.Create;
using BJJMemory.Application.UseCases.Posicoes.Delete;
using BJJMemory.Application.UseCases.Posicoes.Get;
using BJJMemory.Application.UseCases.Posicoes.Update;
using BJJMemory.Communication.Posicoes.Requests;
using BJJMemory.Communication.Posicoes.Responses;
using BJJMemory.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BJJMemory.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PosicaoController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseCreatePosicao), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromServices] ICreatePosicao useCase,
        [FromBody] RequestCreatePosicao request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IList<ResponseGetPosicao>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(
        [FromServices] IGetPosicao useCase,
        [FromQuery] RequestGetPosicaoFilter request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }

    [HttpPut("{posicaoId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid posicaoId,
        [FromServices] IUpdatePosicao useCase,
        [FromBody] RequestUpdatePosicao request)
    {
        await useCase.Execute(posicaoId, request);

        return NoContent();
    }

    [HttpDelete("{posicaoId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid posicaoId,
        [FromServices] IDeletePosicao useCase)
    {
        await useCase.Execute(posicaoId);

        return NoContent();
    }
}
