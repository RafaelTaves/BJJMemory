using BJJMemory.Domain.Repositories;
using BJJMemory.Domain.Repositories.Posicoes;
using BJJMemory.Domain.Services.LoggedUser;
using BJJMemory.Exception;
using BJJMemory.Exception.ExceptionsBase;

namespace BJJMemory.Application.UseCases.Posicoes.Delete;

public class DeletePosicao : IDeletePosicao
{
    private readonly ILoggedUser _loggedUser;
    private readonly IPosicaoWriteOnlyRepository _posicaoWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePosicao(
        ILoggedUser loggedUser,
        IPosicaoWriteOnlyRepository posicaoWriteOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _posicaoWriteOnlyRepository = posicaoWriteOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid posicaoId)
    {
        var user = await _loggedUser.Get();

        var deleted = await _posicaoWriteOnlyRepository.Delete(posicaoId, user.Id);
        if (deleted == false)
        {
            throw new NotFoundException(ResourceErrorMessages.POSITION_NOT_FOUND);
        }

        await _unitOfWork.Commit();
    }
}
