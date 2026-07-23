using BJJMemory.Communication.Categorias.Requests;
using BJJMemory.Domain.Repositories;
using BJJMemory.Domain.Repositories.Categorias;
using BJJMemory.Domain.Services.LoggedUser;
using BJJMemory.Exception;
using BJJMemory.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace BJJMemory.Application.UseCases.Categorias.Update;

public class UpdateCategoria : IUpdateCategoria
{
    private readonly ILoggedUser _loggedUser;
    private readonly ICategoriaReadOnlyRepository _categoriaReadOnlyRepository;
    private readonly ICategoriaUpdateOnlyRepository _categoriaUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoria(
        ILoggedUser loggedUser,
        ICategoriaReadOnlyRepository categoriaReadOnlyRepository,
        ICategoriaUpdateOnlyRepository categoriaUpdateOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _categoriaReadOnlyRepository = categoriaReadOnlyRepository;
        _categoriaUpdateOnlyRepository = categoriaUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid categoriaId, RequestUpdateCategoria request)
    {
        var user = await _loggedUser.Get();
        var categoria = await _categoriaReadOnlyRepository.GetById(categoriaId, user.Id)
            ?? throw new NotFoundException(ResourceErrorMessages.CATEGORY_NOT_FOUND);

        await Validate(request, user.Id, categoriaId);

        categoria.UpdateNome(request.Nome);
        _categoriaUpdateOnlyRepository.Update(categoria);
        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateCategoria request, Guid usuarioId, Guid categoriaId)
    {
        var result = new UpdateCategoriaValidator().Validate(request);

        var categoriaExistente = await _categoriaReadOnlyRepository.ExistCategoriaWithNomeExceptId(usuarioId, request.Nome, categoriaId);
        if (categoriaExistente)
        {
            result.Errors.Add(new ValidationFailure(nameof(request.Nome), ResourceErrorMessages.CATEGORY_NAME_ALREADY_EXISTS));
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(failure => failure.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
