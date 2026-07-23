using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Repositories.Posicoes;
using Microsoft.EntityFrameworkCore;

namespace BJJMemory.Infrastructure.DataAccess.Repositories;

internal class PosicaoRepository : IPosicaoReadOnlyRepository, IPosicaoUpdateOnlyRepository, IPosicaoWriteOnlyRepository
{
    private readonly BJJMemoryDbContext _dbContext;

    public PosicaoRepository(BJJMemoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Posicao posicao)
    {
        await _dbContext.Posicoes.AddAsync(posicao);
    }

    public async Task<bool> Delete(Guid posicaoId, Guid usuarioId)
    {
        var posicao = await _dbContext.Posicoes
            .FirstOrDefaultAsync(item => item.Id == posicaoId && item.UsuarioId == usuarioId);

        if (posicao is null)
        {
            return false;
        }

        _dbContext.Posicoes.Remove(posicao);
        return true;
    }

    public async Task<IList<Posicao>> GetAllByFilters(Guid usuarioId, string? nome, Guid? categoriaId, bool incluirSubcategorias)
    {
        var query = _dbContext.Posicoes
            .AsNoTracking()
            .Where(item => item.UsuarioId == usuarioId);

        if (string.IsNullOrWhiteSpace(nome) == false)
        {
            query = query.Where(item => EF.Functions.ILike(item.Titulo, $"%{nome.Trim()}%"));
        }

        if (categoriaId.HasValue)
        {
            if (incluirSubcategorias)
            {
                query =
                    from posicao in query
                    join categoria in _dbContext.Categorias.AsNoTracking()
                        on posicao.CategoriaId equals categoria.Id
                    where posicao.CategoriaId == categoriaId.Value || categoria.ParentId == categoriaId.Value
                    select posicao;
            }
            else
            {
                query = query.Where(item => item.CategoriaId == categoriaId.Value);
            }
        }

        return await query
            .OrderBy(item => item.Titulo)
            .ToListAsync();
    }

    public async Task<Posicao?> GetById(Guid posicaoId, Guid usuarioId)
    {
        return await _dbContext.Posicoes
            .FirstOrDefaultAsync(item => item.Id == posicaoId && item.UsuarioId == usuarioId);
    }

    public void Update(Posicao posicao)
    {
        _dbContext.Posicoes.Update(posicao);
    }
}
