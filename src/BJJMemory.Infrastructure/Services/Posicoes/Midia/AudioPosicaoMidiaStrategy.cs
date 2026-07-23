using BJJMemory.Domain.Services.Posicoes.Midia;

namespace BJJMemory.Infrastructure.Services.Posicoes.Midia;

internal class AudioPosicaoMidiaStrategy : IAudioPosicaoMidiaStrategy
{
    public string GetInvalidMessage()
    {
        return "O link de áudio informado para a posição é inválido.";
    }

    public bool IsValid(string? link)
    {
        if (string.IsNullOrWhiteSpace(link))
        {
            return true;
        }

        var isValidUrl = Uri.TryCreate(link, UriKind.Absolute, out var uri);
        return isValidUrl && (uri!.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }

    public string? Normalize(string? link)
    {
        if (string.IsNullOrWhiteSpace(link))
        {
            return null;
        }

        return link.Trim();
    }
}
