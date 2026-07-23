namespace BJJMemory.Domain.Services.Posicoes.Midia;

public interface IPosicaoMidiaStrategy
{
    string? Normalize(string? link);

    bool IsValid(string? link);

    string GetInvalidMessage();
}
