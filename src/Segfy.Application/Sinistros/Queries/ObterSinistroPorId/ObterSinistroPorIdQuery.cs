using MediatR;
using Segfy.Application.Sinistros.Dtos;

namespace Segfy.Application.Sinistros.Queries.ObterSinistroPorId;

public sealed record ObterSinistroPorIdQuery(Guid Id) : IRequest<SinistroDto>;
