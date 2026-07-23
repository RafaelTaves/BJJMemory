using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Repositories.Posicoes;

namespace BJJMemory.Application.Tests.Common.Fakes;

public sealed class PosicaoReadOnlyRepositorySpy : IPosicaoReadOnlyRepository
{
    public string? ReceivedNome { get; private set; }

    public Guid? ReceivedCategoriaId { get; private set; }

    public bool ReceivedIncluirSubcategorias { get; private set; }

    public IList<Posicao> GetAllByFiltersResult { get; set; } = [];

    public Dictionary<Guid, Posicao> PosicoesById { get; } = [];

    public Task<IList<Posicao>> GetAllByFilters(Guid usuarioId, string? nome, Guid? categoriaId, bool incluirSubcategorias)
    {
        ReceivedNome = nome;
        ReceivedCategoriaId = categoriaId;
        ReceivedIncluirSubcategorias = incluirSubcategorias;
        return Task.FromResult(GetAllByFiltersResult);
    }

    public Task<Posicao?> GetById(Guid posicaoId, Guid usuarioId)
    {
        if (PosicoesById.TryGetValue(posicaoId, out var posicao) && posicao.UsuarioId == usuarioId)
        {
            return Task.FromResult<Posicao?>(posicao);
        }

        return Task.FromResult<Posicao?>(null);
    }
}
