namespace BJJMemory.Communication.Posicoes.Responses;

public class ResponseGetPosicao
{
    public Guid Id { get; set; }

    public Guid CategoriaId { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public string? AudioLink { get; set; }

    public string? VideoLink { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
