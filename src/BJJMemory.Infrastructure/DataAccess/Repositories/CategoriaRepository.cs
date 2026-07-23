using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Repositories.Categorias;
using Microsoft.EntityFrameworkCore;

namespace BJJMemory.Infrastructure.DataAccess.Repositories;

internal class CategoriaRepository : ICategoriaReadOnlyRepository, ICategoriaUpdateOnlyRepository, ICategoriaWriteOnlyRepository
{
    private readonly BJJMemoryDbContext _dbContext;

    public CategoriaRepository(BJJMemoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Categoria categoria)
    {
        await _dbContext.Categorias.AddAsync(categoria);
    }

    public async Task<bool> Delete(Guid categoriaId, Guid usuarioId)
    {
        var categoria = await _dbContext.Categorias
            .FirstOrDefaultAsync(item => item.Id == categoriaId && item.UsuarioId == usuarioId);

        if (categoria is null)
        {
            return false;
        }

        _dbContext.Categorias.Remove(categoria);
        return true;
    }

    public async Task<bool> ExistCategoriaWithNome(Guid usuarioId, string nome)
    {
        return await _dbContext.Categorias
            .AnyAsync(item => item.UsuarioId == usuarioId && item.Nome == nome);
    }

    public async Task<bool> ExistCategoriaWithNomeExceptId(Guid usuarioId, string nome, Guid categoriaId)
    {
        return await _dbContext.Categorias
            .AnyAsync(item => item.UsuarioId == usuarioId && item.Nome == nome && item.Id != categoriaId);
    }

    public async Task<IList<Categoria>> GetAllByUsuarioId(Guid usuarioId)
    {
        return await _dbContext.Categorias
            .AsNoTracking()
            .Where(item => item.UsuarioId == usuarioId)
            .ToListAsync();
    }

    public async Task<Categoria?> GetById(Guid categoriaId, Guid usuarioId)
    {
        return await _dbContext.Categorias
            .FirstOrDefaultAsync(item => item.Id == categoriaId && item.UsuarioId == usuarioId);
    }

    public void Update(Categoria categoria)
    {
        _dbContext.Categorias.Update(categoria);
    }
}
