using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Repositories.Usuarios;
using Microsoft.EntityFrameworkCore;

namespace BJJMemory.Infrastructure.DataAccess.Repositories;

internal class UsuarioRepository : IUsuarioReadOnlyRepository, IUsuarioUpdateOnlyRepository, IUsuarioWriteOnlyRepository
{
    private readonly BJJMemoryDbContext _dbContext;

    public UsuarioRepository(BJJMemoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Usuario user)
    {
        await _dbContext.Set<Usuario>().AddAsync(user);
    }

    public async Task<bool> ExistUserWithEmail(string email)
    {
        return await _dbContext.Usuarios.AnyAsync(user => user.Email == email);
    }

    public async Task<Usuario> GetUserByEmail(string email)
    {
        return await _dbContext.Usuarios.AsNoTracking().FirstOrDefaultAsync(user => user.Email == email);
    }

    public void Update(Usuario user)
    {
        _dbContext.Usuarios.Update(user);
    }

    public async Task<bool> Delete(Guid id)
    {
        var user = await _dbContext.Usuarios.FirstOrDefaultAsync(user => user.Id == id);
        if (user == null)
        {
            return false;
        }

        _dbContext.Usuarios.Remove(user);
        return true;
    }
}
