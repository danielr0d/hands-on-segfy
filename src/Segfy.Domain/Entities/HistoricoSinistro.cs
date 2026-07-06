using Segfy.Domain.Common;
using Segfy.Domain.Enums;

namespace Segfy.Domain.Entities;

public class HistoricoSinistro : BaseEntity
{
    public Guid SinistroId { get; private set; }
    public DateTime DataAlteracao { get; private set; }
    public SinistroStatus StatusAnterior { get; private set; }
    public SinistroStatus StatusNovo { get; private set; }

    protected HistoricoSinistro()
    {
    }

    internal HistoricoSinistro(Guid sinistroId, SinistroStatus statusAnterior, SinistroStatus statusNovo)
    {
        SinistroId = sinistroId;
        StatusAnterior = statusAnterior;
        StatusNovo = statusNovo;
        DataAlteracao = DateTime.UtcNow;
    }
}
