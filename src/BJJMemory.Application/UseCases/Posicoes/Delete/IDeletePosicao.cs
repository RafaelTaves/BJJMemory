namespace BJJMemory.Application.UseCases.Posicoes.Delete;

public interface IDeletePosicao
{
    Task Execute(Guid posicaoId);
}
