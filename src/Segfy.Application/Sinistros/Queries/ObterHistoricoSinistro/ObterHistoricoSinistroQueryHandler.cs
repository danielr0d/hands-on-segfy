using MediatR;
using Segfy.Application.Common.Exceptions;
using Segfy.Application.Common.Interfaces;
using Segfy.Application.Sinistros.Dtos;
using Segfy.Domain.Entities;

namespace Segfy.Application.Sinistros.Queries.ObterHistoricoSinistro;

public sealed class ObterHistoricoSinistroQueryHandler
    : IRequestHandler<ObterHistoricoSinistroQuery, IReadOnlyCollection<HistoricoSinistroDto>>
{
    private readonly ISinistroRepository _sinistroRepository;

    public ObterHistoricoSinistroQueryHandler(ISinistroRepository sinistroRepository)
    {
        _sinistroRepository = sinistroRepository;
    }

    public async Task<IReadOnlyCollection<HistoricoSinistroDto>> Handle(
        ObterHistoricoSinistroQuery request, CancellationToken cancellationToken)
    {
        var sinistro = await _sinistroRepository.ObterComHistoricoPorIdAsync(request.SinistroId, cancellationToken)
            ?? throw new NotFoundException(nameof(Sinistro), request.SinistroId);

        return sinistro.Historico.Select(HistoricoSinistroDto.FromEntity).ToList();
    }
}
