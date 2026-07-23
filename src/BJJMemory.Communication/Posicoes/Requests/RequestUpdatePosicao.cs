namespace BJJMemory.Communication.Posicoes.Requests;

public class RequestUpdatePosicao
{
    public Guid CategoriaId { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public string? AudioLink { get; set; }

    public string? VideoLink { get; set; }
}
