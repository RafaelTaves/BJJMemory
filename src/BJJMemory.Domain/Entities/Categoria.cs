namespace BJJMemory.Domain.Entities;

public class Categoria
{
    public Guid Id { get; set; }

    public Guid UsuarioId { get; set; }

    public Guid? ParentId { get; set; }

    public string Nome { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
