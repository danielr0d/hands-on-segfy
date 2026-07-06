using Microsoft.EntityFrameworkCore;
using Segfy.Application.Common.Interfaces;
using Segfy.Domain.Entities;
using Segfy.Domain.Enums;

namespace Segfy.Infrastructure.Persistence.Repositories;

public class SinistroRepository : ISinistroRepository
{
    private readonly SegfyDbContext _dbContext;

    public SinistroRepository(SegfyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Sinistro?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken) =>
        _dbContext.Sinistros.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public Task<Sinistro?> ObterComHistoricoPorIdAsync(Guid id, CancellationToken cancellationToken) =>
        _dbContext.Sinistros
            .Include(s => s.Historico)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public async Task AdicionarAsync(Sinistro sinistro, CancellationToken cancellationToken) =>
        await _dbContext.Sinistros.AddAsync(sinistro, cancellationToken);

    public async Task<(IReadOnlyCollection<Sinistro> Itens, int TotalRegistros)> ListarAsync(
        SinistroStatus? status,
        DateTime? dataInicio,
        DateTime? dataFim,
        int pagina,
        int tamanhoPagina,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Sinistros.AsNoTracking().AsQueryable();

        if (status is not null)
            query = query.Where(s => s.Status == status);

        if (dataInicio is not null)
            query = query.Where(s => s.DataAbertura >= dataInicio.Value);

        if (dataFim is not null)
            query = query.Where(s => s.DataAbertura <= dataFim.Value);

        var totalRegistros = await query.CountAsync(cancellationToken);

        var itens = await query
            .OrderByDescending(s => s.DataAbertura)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync(cancellationToken);

        return (itens, totalRegistros);
    }
}
