using Segfy.Domain.Entities;
using Segfy.Domain.Enums;

namespace Segfy.Application.Sinistros.Dtos;

public sealed record SinistroDto(
    Guid Id,
    Guid ApoliceId,
    DateTime DataAbertura,
    SinistroStatus Status,
    decimal ValorEstimado,
    decimal? ValorAprovado,
    string? MotivoNegativa)
{
    public static SinistroDto FromEntity(Sinistro sinistro) => new(
        sinistro.Id,
        sinistro.ApoliceId,
        sinistro.DataAbertura,
        sinistro.Status,
        sinistro.ValorEstimado,
        sinistro.ValorAprovado?.Valor,
        sinistro.MotivoNegativa);
}
