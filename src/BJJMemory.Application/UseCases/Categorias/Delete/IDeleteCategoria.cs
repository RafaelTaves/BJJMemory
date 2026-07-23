namespace BJJMemory.Application.UseCases.Categorias.Delete;

public interface IDeleteCategoria
{
    Task Execute(Guid categoriaId);
}
