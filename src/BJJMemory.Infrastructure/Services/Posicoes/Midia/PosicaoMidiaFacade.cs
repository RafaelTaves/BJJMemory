using BJJMemory.Domain.Services.Posicoes.Midia;

namespace BJJMemory.Infrastructure.Services.Posicoes.Midia;

internal class PosicaoMidiaFacade : IPosicaoMidiaFacade
{
    private readonly IAudioPosicaoMidiaStrategy _audioPosicaoMidiaStrategy;
    private readonly IVideoPosicaoMidiaStrategy _videoPosicaoMidiaStrategy;

    public PosicaoMidiaFacade(
        IAudioPosicaoMidiaStrategy audioPosicaoMidiaStrategy,
        IVideoPosicaoMidiaStrategy videoPosicaoMidiaStrategy)
    {
        _audioPosicaoMidiaStrategy = audioPosicaoMidiaStrategy;
        _videoPosicaoMidiaStrategy = videoPosicaoMidiaStrategy;
    }

    public PosicaoMidiaResult Process(string? audioLink, string? videoLink)
    {
        var normalizedAudioLink = _audioPosicaoMidiaStrategy.Normalize(audioLink);
        var normalizedVideoLink = _videoPosicaoMidiaStrategy.Normalize(videoLink);
        var errors = new List<string>();

        if (_audioPosicaoMidiaStrategy.IsValid(normalizedAudioLink) == false)
        {
            errors.Add(_audioPosicaoMidiaStrategy.GetInvalidMessage());
        }

        if (_videoPosicaoMidiaStrategy.IsValid(normalizedVideoLink) == false)
        {
            errors.Add(_videoPosicaoMidiaStrategy.GetInvalidMessage());
        }

        return new PosicaoMidiaResult
        {
            AudioLink = normalizedAudioLink,
            VideoLink = normalizedVideoLink,
            Errors = errors
        };
    }
}
