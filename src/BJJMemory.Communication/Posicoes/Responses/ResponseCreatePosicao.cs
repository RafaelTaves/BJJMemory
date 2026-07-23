namespace BJJMemory.Communication.Posicoes.Responses;

public class ResponseCreatePosicao
{
    public Guid Id { get; set; }

    public Guid CategoriaId { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public string? AudioLink { get; set; }

    public string? VideoLink { get; set; }
}
