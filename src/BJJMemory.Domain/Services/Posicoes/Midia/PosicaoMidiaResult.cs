namespace BJJMemory.Domain.Services.Posicoes.Midia;

public class PosicaoMidiaResult
{
    public string? AudioLink { get; set; }

    public string? VideoLink { get; set; }

    public IList<string> Errors { get; set; } = [];
}
