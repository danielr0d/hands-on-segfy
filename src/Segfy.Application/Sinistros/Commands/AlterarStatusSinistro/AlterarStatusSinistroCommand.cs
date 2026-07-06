using MediatR;
using Segfy.Application.Sinistros.Dtos;
using Segfy.Domain.Enums;

namespace Segfy.Application.Sinistros.Commands.AlterarStatusSinistro;

public sealed record AlterarStatusSinistroCommand(
    Guid SinistroId,
    SinistroStatus NovoStatus,
    decimal? ValorAprovado,
    string? MotivoNegativa) : IRequest<SinistroDto>;
