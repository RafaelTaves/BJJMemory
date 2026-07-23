using BJJMemory.Domain.Services.Posicoes.Midia;

namespace BJJMemory.Application.Tests.Common.Fakes;

public sealed class PosicaoMidiaFacadeFake : IPosicaoMidiaFacade
{
    public string? ReceivedAudioLink { get; private set; }

    public string? ReceivedVideoLink { get; private set; }

    public PosicaoMidiaResult ProcessResult { get; set; } = new();

    public PosicaoMidiaResult Process(string? audioLink, string? videoLink)
    {
        ReceivedAudioLink = audioLink;
        ReceivedVideoLink = videoLink;
        return ProcessResult;
    }
}
