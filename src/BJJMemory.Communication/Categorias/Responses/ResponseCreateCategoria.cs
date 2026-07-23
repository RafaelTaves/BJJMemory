namespace BJJMemory.Communication.Categorias.Responses;

public class ResponseCreateCategoria
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public Guid? ParentId { get; set; }
}
