using MediatR;
using Segfy.Application.Common.Models;
using Segfy.Application.Sinistros.Dtos;
using Segfy.Domain.Enums;

namespace Segfy.Application.Sinistros.Queries.ListarSinistros;

public sealed record ListarSinistrosQuery(
    SinistroStatus? Status,
    DateTime? DataInicio,
    DateTime? DataFim,
    int Pagina = 1,
    int TamanhoPagina = 10) : IRequest<PagedResult<SinistroDto>>;
