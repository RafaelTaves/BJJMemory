namespace BJJMemory.Communication.Categorias.Responses;

public class ResponseCategoriaTree
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public IList<ResponseCategoriaTree> Subcategorias { get; set; } = [];
}
