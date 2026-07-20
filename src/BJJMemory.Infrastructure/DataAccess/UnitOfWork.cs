using BJJMemory.Domain.Repositories;

namespace BJJMemory.Infrastructure.DataAccess;

internal class UnitOfWork : IUnitOfWork
{
    private readonly BJJMemoryDbContext _dbContext;

    public UnitOfWork(BJJMemoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Commit()
    {
        await _dbContext.SaveChangesAsync();
    }
}
