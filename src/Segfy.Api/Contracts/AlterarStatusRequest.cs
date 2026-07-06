using Segfy.Domain.Enums;

namespace Segfy.Api.Contracts;

public sealed record AlterarStatusRequest(SinistroStatus NovoStatus, decimal? ValorAprovado, string? MotivoNegativa);
