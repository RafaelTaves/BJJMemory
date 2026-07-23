using BJJMemory.Communication.Categorias.Responses;

namespace BJJMemory.Application.UseCases.Categorias.Get;

public interface IGetCategoria
{
    Task<IList<ResponseCategoriaTree>> Execute();
}
