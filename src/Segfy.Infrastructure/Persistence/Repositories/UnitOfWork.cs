using Segfy.Application.Common.Interfaces;

namespace Segfy.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly SegfyDbContext _dbContext;

    public UnitOfWork(SegfyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken) =>
        _dbContext.SaveChangesAsync(cancellationToken);
}
