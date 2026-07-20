namespace BJJMemory.Domain.Repositories;

public interface IUnitOfWork
{
    Task Commit();
}
