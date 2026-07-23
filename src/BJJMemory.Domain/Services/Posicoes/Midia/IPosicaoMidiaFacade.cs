namespace BJJMemory.Domain.Services.Posicoes.Midia;

public interface IPosicaoMidiaFacade
{
    PosicaoMidiaResult Process(string? audioLink, string? videoLink);
}
