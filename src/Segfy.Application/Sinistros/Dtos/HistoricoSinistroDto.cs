using Segfy.Domain.Entities;
using Segfy.Domain.Enums;

namespace Segfy.Application.Sinistros.Dtos;

public sealed record HistoricoSinistroDto(
    Guid Id,
    Guid SinistroId,
    DateTime DataAlteracao,
    SinistroStatus StatusAnterior,
    SinistroStatus StatusNovo)
{
    public static HistoricoSinistroDto FromEntity(HistoricoSinistro historico) => new(
        historico.Id,
        historico.SinistroId,
        historico.DataAlteracao,
        historico.StatusAnterior,
        historico.StatusNovo);
}
