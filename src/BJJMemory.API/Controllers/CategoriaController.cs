using BJJMemory.Application.UseCases.Categorias.Create;
using BJJMemory.Application.UseCases.Categorias.Delete;
using BJJMemory.Application.UseCases.Categorias.Get;
using BJJMemory.Application.UseCases.Categorias.Update;
using BJJMemory.Communication.Categorias.Requests;
using BJJMemory.Communication.Categorias.Responses;
using BJJMemory.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BJJMemory.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriaController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseCreateCategoria), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromServices] ICreateCategoria useCase,
        [FromBody] RequestCreateCategoria request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IList<ResponseCategoriaTree>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromServices] IGetCategoria useCase)
    {
        var response = await useCase.Execute();

        return Ok(response);
    }

    [HttpPut("{categoriaId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid categoriaId,
        [FromServices] IUpdateCategoria useCase,
        [FromBody] RequestUpdateCategoria request)
    {
        await useCase.Execute(categoriaId, request);

        return NoContent();
    }

    [HttpDelete("{categoriaId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid categoriaId,
        [FromServices] IDeleteCategoria useCase)
    {
        await useCase.Execute(categoriaId);

        return NoContent();
    }
}
