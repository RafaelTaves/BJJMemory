namespace BJJMemory.Communication.Categorias.Requests;

public class RequestCreateCategoria
{
    public string Nome { get; set; } = string.Empty;

    public Guid? ParentId { get; set; }
}
