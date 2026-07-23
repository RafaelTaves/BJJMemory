namespace BJJMemory.Domain.Entities;

public class Posicao
{
    public Guid Id { get; set; }

    public Guid UsuarioId { get; set; }

    public Guid CategoriaId { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public string? AudioLink { get; set; }

    public string? VideoLink { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    private Posicao() { }

    private Posicao(
        Guid id,
        Guid usuarioId,
        Guid categoriaId,
        string titulo,
        string descricao,
        string? audioLink,
        string? videoLink,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        UsuarioId = usuarioId;
        CategoriaId = categoriaId;
        Titulo = titulo;
        Descricao = descricao;
        AudioLink = audioLink;
        VideoLink = videoLink;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Posicao Create(
        Guid usuarioId,
        Guid categoriaId,
        string titulo,
        string descricao,
        string? audioLink,
        string? videoLink)
    {
        var now = DateTime.UtcNow;

        return new Posicao(Guid.NewGuid(), usuarioId, categoriaId, titulo, descricao, audioLink, videoLink, now, now);
    }

    public void Update(
        Guid categoriaId,
        string titulo,
        string descricao,
        string? audioLink,
        string? videoLink)
    {
        CategoriaId = categoriaId;
        Titulo = titulo;
        Descricao = descricao;
        AudioLink = audioLink;
        VideoLink = videoLink;
        UpdatedAt = DateTime.UtcNow;
    }
}
