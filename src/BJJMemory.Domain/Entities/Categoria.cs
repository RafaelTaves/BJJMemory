namespace BJJMemory.Domain.Entities;

public class Categoria
{
    public Guid Id { get; set; }

    public Guid UsuarioId { get; set; }

    public Guid? ParentId { get; set; }

    public string Nome { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Categoria? Parent { get; set; }

    public ICollection<Categoria> Subcategorias { get; set; } = [];

    private Categoria() { }

    private Categoria(Guid id, Guid usuarioId, Guid? parentId, string nome, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        UsuarioId = usuarioId;
        ParentId = parentId;
        Nome = nome;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Categoria Create(Guid usuarioId, string nome, Guid? parentId)
    {
        var now = DateTime.UtcNow;

        return new Categoria(Guid.NewGuid(), usuarioId, parentId, nome, now, now);
    }

    public void UpdateNome(string nome)
    {
        Nome = nome;
        UpdatedAt = DateTime.UtcNow;
    }
}
