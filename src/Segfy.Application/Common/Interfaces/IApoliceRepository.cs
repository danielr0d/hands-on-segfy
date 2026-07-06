using Segfy.Domain.Entities;

namespace Segfy.Application.Common.Interfaces;

public interface IApoliceRepository
{
    Task<Apolice?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
}
