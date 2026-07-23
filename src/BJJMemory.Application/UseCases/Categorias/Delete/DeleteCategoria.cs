using BJJMemory.Domain.Repositories;
using BJJMemory.Domain.Repositories.Categorias;
using BJJMemory.Domain.Services.LoggedUser;
using BJJMemory.Exception;
using BJJMemory.Exception.ExceptionsBase;

namespace BJJMemory.Application.UseCases.Categorias.Delete;

public class DeleteCategoria : IDeleteCategoria
{
    private readonly ILoggedUser _loggedUser;
    private readonly ICategoriaWriteOnlyRepository _categoriaWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoria(
        ILoggedUser loggedUser,
        ICategoriaWriteOnlyRepository categoriaWriteOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _categoriaWriteOnlyRepository = categoriaWriteOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid categoriaId)
    {
        var user = await _loggedUser.Get();

        var deleted = await _categoriaWriteOnlyRepository.Delete(categoriaId, user.Id);
        if (deleted == false)
        {
            throw new NotFoundException(ResourceErrorMessages.CATEGORY_NOT_FOUND);
        }

        await _unitOfWork.Commit();
    }
}
