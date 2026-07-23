using BJJMemory.Communication.Posicoes.Requests;
using BJJMemory.Communication.Posicoes.Responses;
using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Repositories.Categorias;
using BJJMemory.Domain.Repositories.Posicoes;
using BJJMemory.Domain.Services.LoggedUser;
using BJJMemory.Exception;
using BJJMemory.Exception.ExceptionsBase;

namespace BJJMemory.Application.UseCases.Posicoes.Get;

public class GetPosicao : IGetPosicao
{
    private readonly ILoggedUser _loggedUser;
    private readonly IPosicaoReadOnlyRepository _posicaoReadOnlyRepository;
    private readonly ICategoriaReadOnlyRepository _categoriaReadOnlyRepository;

    public GetPosicao(
        ILoggedUser loggedUser,
        IPosicaoReadOnlyRepository posicaoReadOnlyRepository,
        ICategoriaReadOnlyRepository categoriaReadOnlyRepository)
    {
        _loggedUser = loggedUser;
        _posicaoReadOnlyRepository = posicaoReadOnlyRepository;
        _categoriaReadOnlyRepository = categoriaReadOnlyRepository;
    }

    public async Task<IList<ResponseGetPosicao>> Execute(RequestGetPosicaoFilter request)
    {
        var user = await _loggedUser.Get();
        var categoriaId = request.SubcategoriaId ?? request.CategoriaId;
        var incluirSubcategorias = request.SubcategoriaId.HasValue == false && request.CategoriaId.HasValue;

        if (categoriaId.HasValue)
        {
            var categoria = await _categoriaReadOnlyRepository.GetById(categoriaId.Value, user.Id);
            if (categoria is null)
            {
                throw new ErrorOnValidationException([ResourceErrorMessages.POSITION_CATEGORY_NOT_FOUND]);
            }
        }

        var posicoes = await _posicaoReadOnlyRepository.GetAllByFilters(
            user.Id,
            request.Nome,
            categoriaId,
            incluirSubcategorias);

        return posicoes.Select(MapToResponse).ToList();
    }

    private static ResponseGetPosicao MapToResponse(Posicao posicao)
    {
        return new ResponseGetPosicao
        {
            Id = posicao.Id,
            CategoriaId = posicao.CategoriaId,
            Titulo = posicao.Titulo,
            Descricao = posicao.Descricao,
            AudioLink = posicao.AudioLink,
            VideoLink = posicao.VideoLink,
            CreatedAt = posicao.CreatedAt,
            UpdatedAt = posicao.UpdatedAt
        };
    }
}
