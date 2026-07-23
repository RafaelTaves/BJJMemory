using BJJMemory.Communication.Categorias.Requests;
using BJJMemory.Communication.Categorias.Responses;
using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Repositories;
using BJJMemory.Domain.Repositories.Categorias;
using BJJMemory.Domain.Services.LoggedUser;
using BJJMemory.Exception;
using BJJMemory.Exception.ExceptionsBase;
using FluentValidation;
using FluentValidation.Results;

namespace BJJMemory.Application.UseCases.Categorias.Create;

public class CreateCategoria : ICreateCategoria
{
    private readonly ILoggedUser _loggedUser;
    private readonly ICategoriaReadOnlyRepository _categoriaReadOnlyRepository;
    private readonly ICategoriaWriteOnlyRepository _categoriaWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RequestCreateCategoria> _validator;

    public CreateCategoria(
        ILoggedUser loggedUser,
        ICategoriaReadOnlyRepository categoriaReadOnlyRepository,
        ICategoriaWriteOnlyRepository categoriaWriteOnlyRepository,
        IUnitOfWork unitOfWork,
        IValidator<RequestCreateCategoria> validator)
    {
        _loggedUser = loggedUser;
        _categoriaReadOnlyRepository = categoriaReadOnlyRepository;
        _categoriaWriteOnlyRepository = categoriaWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<ResponseCreateCategoria> Execute(RequestCreateCategoria request)
    {
        var user = await _loggedUser.Get();

        await Validate(request, user.Id);

        var categoria = Categoria.Create(user.Id, request.Nome, request.ParentId);

        await _categoriaWriteOnlyRepository.Add(categoria);
        await _unitOfWork.Commit();

        return new ResponseCreateCategoria
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
            ParentId = categoria.ParentId
        };
    }

    private async Task Validate(RequestCreateCategoria request, Guid usuarioId)
    {
        var result = await _validator.ValidateAsync(request);

        var categoriaExistente = await _categoriaReadOnlyRepository.ExistCategoriaWithNome(usuarioId, request.Nome);
        if (categoriaExistente)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.CATEGORY_NAME_ALREADY_EXISTS));
        }

        if (request.ParentId.HasValue)
        {
            var parent = await _categoriaReadOnlyRepository.GetById(request.ParentId.Value, usuarioId);
            if (parent is null)
            {
                result.Errors.Add(new ValidationFailure(nameof(request.ParentId), ResourceErrorMessages.CATEGORY_PARENT_NOT_FOUND));
            }
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(failure => failure.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
