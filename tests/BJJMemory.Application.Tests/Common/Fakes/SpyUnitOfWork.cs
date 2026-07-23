using BJJMemory.Domain.Repositories;

namespace BJJMemory.Application.Tests.Common.Fakes;

public sealed class SpyUnitOfWork : IUnitOfWork
{
    public int CommitCalls { get; private set; }

    public Task Commit()
    {
        CommitCalls++;
        return Task.CompletedTask;
    }
}
