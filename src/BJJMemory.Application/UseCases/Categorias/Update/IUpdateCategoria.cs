using BJJMemory.Communication.Categorias.Requests;

namespace BJJMemory.Application.UseCases.Categorias.Update;

public interface IUpdateCategoria
{
    Task Execute(Guid categoriaId, RequestUpdateCategoria request);
}
