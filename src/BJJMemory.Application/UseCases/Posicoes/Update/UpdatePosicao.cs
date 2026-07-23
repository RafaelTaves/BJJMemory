using BJJMemory.Communication.Posicoes.Requests;
using BJJMemory.Domain.Repositories;
using BJJMemory.Domain.Repositories.Categorias;
using BJJMemory.Domain.Repositories.Posicoes;
using BJJMemory.Domain.Services.LoggedUser;
using BJJMemory.Domain.Services.Posicoes.Midia;
using BJJMemory.Exception;
using BJJMemory.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace BJJMemory.Application.UseCases.Posicoes.Update;

public class UpdatePosicao : IUpdatePosicao
{
    private readonly ILoggedUser _loggedUser;
    private readonly ICategoriaReadOnlyRepository _categoriaReadOnlyRepository;
    private readonly IPosicaoReadOnlyRepository _posicaoReadOnlyRepository;
    private readonly IPosicaoUpdateOnlyRepository _posicaoUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPosicaoMidiaFacade _posicaoMidiaFacade;

    public UpdatePosicao(
        ILoggedUser loggedUser,
        ICategoriaReadOnlyRepository categoriaReadOnlyRepository,
        IPosicaoReadOnlyRepository posicaoReadOnlyRepository,
        IPosicaoUpdateOnlyRepository posicaoUpdateOnlyRepository,
        IUnitOfWork unitOfWork,
        IPosicaoMidiaFacade posicaoMidiaFacade)
    {
        _loggedUser = loggedUser;
        _categoriaReadOnlyRepository = categoriaReadOnlyRepository;
        _posicaoReadOnlyRepository = posicaoReadOnlyRepository;
        _posicaoUpdateOnlyRepository = posicaoUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
        _posicaoMidiaFacade = posicaoMidiaFacade;
    }

    public async Task Execute(Guid posicaoId, RequestUpdatePosicao request)
    {
        var user = await _loggedUser.Get();
        var posicao = await _posicaoReadOnlyRepository.GetById(posicaoId, user.Id)
            ?? throw new NotFoundException(ResourceErrorMessages.POSITION_NOT_FOUND);

        var midiaResult = await Validate(request, user.Id);

        posicao.Update(
            request.CategoriaId,
            request.Titulo,
            request.Descricao,
            midiaResult.AudioLink,
            midiaResult.VideoLink);

        _posicaoUpdateOnlyRepository.Update(posicao);
        await _unitOfWork.Commit();
    }

    private async Task<PosicaoMidiaResult> Validate(RequestUpdatePosicao request, Guid usuarioId)
    {
        var result = new UpdatePosicaoValidator().Validate(request);

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
}
