namespace BJJMemory.Domain.Entities;

public class Posicoes
{
    public Guid Id { get; set; }

    public Guid CategoriaId { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public string? AudioLink { get; set; } = string.Empty;

    public string? VideoLink { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
