using BJJMemory.Communication.Posicoes.Requests;
using BJJMemory.Communication.Posicoes.Responses;
using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Repositories;
using BJJMemory.Domain.Repositories.Categorias;
using BJJMemory.Domain.Repositories.Posicoes;
using BJJMemory.Domain.Services.LoggedUser;
using BJJMemory.Domain.Services.Posicoes.Midia;
using BJJMemory.Exception;
using BJJMemory.Exception.ExceptionsBase;
using FluentValidation;
using FluentValidation.Results;

namespace BJJMemory.Application.UseCases.Posicoes.Create;

public class CreatePosicao : ICreatePosicao
{
    private readonly ILoggedUser _loggedUser;
    private readonly ICategoriaReadOnlyRepository _categoriaReadOnlyRepository;
    private readonly IPosicaoWriteOnlyRepository _posicaoWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RequestCreatePosicao> _validator;
    private readonly IPosicaoMidiaFacade _posicaoMidiaFacade;

    public CreatePosicao(
        ILoggedUser loggedUser,
        ICategoriaReadOnlyRepository categoriaReadOnlyRepository,
        IPosicaoWriteOnlyRepository posicaoWriteOnlyRepository,
        IUnitOfWork unitOfWork,
        IValidator<RequestCreatePosicao> validator,
        IPosicaoMidiaFacade posicaoMidiaFacade)
    {
        _loggedUser = loggedUser;
        _categoriaReadOnlyRepository = categoriaReadOnlyRepository;
        _posicaoWriteOnlyRepository = posicaoWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _posicaoMidiaFacade = posicaoMidiaFacade;
    }

    public async Task<ResponseCreatePosicao> Execute(RequestCreatePosicao request)
    {
        var user = await _loggedUser.Get();

        var midiaResult = await Validate(request, user.Id);

        var posicao = MapToEntity(request, user.Id, midiaResult);

        await _posicaoWriteOnlyRepository.Add(posicao);
        await _unitOfWork.Commit();

        return MapToResponse(posicao);
    }

    private async Task<PosicaoMidiaResult> Validate(RequestCreatePosicao request, Guid usuarioId)
    {
        var result = await _validator.ValidateAsync(request);

        var categoria = await _categoriaReadOnlyRepository.GetById(request.CategoriaId, usuarioId);
        if (categoria is null)
        {
            result.Errors.Add(new ValidationFailure(nameof(request.CategoriaId), ResourceErrorMessages.POSITION_CATEGORY_NOT_FOUND));
        }

        var midiaResult = _posicaoMidiaFacade.Process(request.AudioLink, request.VideoLink);
        foreach (var error in midiaResult.Errors)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, error));
        }

        if (result.IsValid == false)
        {
            throw new ErrorOnValidationException(result.Errors.Select(failure => failure.ErrorMessage).ToList());
        }

        return midiaResult;
    }

    private static Posicao MapToEntity(RequestCreatePosicao request, Guid usuarioId, PosicaoMidiaResult midia)
    {
        return Posicao.Create(
            usuarioId,
            request.CategoriaId,
            request.Titulo,
            request.Descricao,
            midia.AudioLink,
            midia.VideoLink);
    }

    private static ResponseCreatePosicao MapToResponse(Posicao posicao)
    {
        return new ResponseCreatePosicao
        {
            Id = posicao.Id,
            CategoriaId = posicao.CategoriaId,
            Titulo = posicao.Titulo,
            Descricao = posicao.Descricao,
            AudioLink = posicao.AudioLink,
            VideoLink = posicao.VideoLink
        };
    }
}
