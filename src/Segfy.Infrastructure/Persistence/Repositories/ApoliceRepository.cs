using Microsoft.EntityFrameworkCore;
using Segfy.Application.Common.Interfaces;
using Segfy.Domain.Entities;

namespace Segfy.Infrastructure.Persistence.Repositories;

public class ApoliceRepository : IApoliceRepository
{
    private readonly SegfyDbContext _dbContext;

    public ApoliceRepository(SegfyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Apolice?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken) =>
        _dbContext.Apolices.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
}
