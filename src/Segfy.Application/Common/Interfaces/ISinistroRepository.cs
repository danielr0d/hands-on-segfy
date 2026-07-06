using Segfy.Domain.Entities;
using Segfy.Domain.Enums;

namespace Segfy.Application.Common.Interfaces;

public interface ISinistroRepository
{
    Task<Sinistro?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Sinistro?> ObterComHistoricoPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task AdicionarAsync(Sinistro sinistro, CancellationToken cancellationToken);

    Task<(IReadOnlyCollection<Sinistro> Itens, int TotalRegistros)> ListarAsync(
        SinistroStatus? status,
        DateTime? dataInicio,
        DateTime? dataFim,
        int pagina,
        int tamanhoPagina,
        CancellationToken cancellationToken);
}
