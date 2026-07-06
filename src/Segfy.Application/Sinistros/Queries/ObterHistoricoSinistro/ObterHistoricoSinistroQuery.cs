using MediatR;
using Segfy.Application.Sinistros.Dtos;

namespace Segfy.Application.Sinistros.Queries.ObterHistoricoSinistro;

public sealed record ObterHistoricoSinistroQuery(Guid SinistroId) : IRequest<IReadOnlyCollection<HistoricoSinistroDto>>;
