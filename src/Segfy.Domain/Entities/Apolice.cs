using Segfy.Domain.Common;
using Segfy.Domain.Enums;
using Segfy.Domain.Exceptions;

namespace Segfy.Domain.Entities;

public class Apolice : BaseEntity
{
    public string Numero { get; private set; } = null!;
    public ApoliceStatus Status { get; private set; }
    public Guid ClienteId { get; private set; }
    public Guid RamoId { get; private set; }

    protected Apolice()
    {
    }

    public Apolice(string numero, Guid clienteId, Guid ramoId)
    {
        if (string.IsNullOrWhiteSpace(numero))
            throw new CampoObrigatorioException(nameof(Numero));

        if (clienteId == Guid.Empty)
            throw new CampoObrigatorioException(nameof(ClienteId));

        if (ramoId == Guid.Empty)
            throw new CampoObrigatorioException(nameof(RamoId));

        Numero = numero;
        ClienteId = clienteId;
        RamoId = ramoId;
        Status = ApoliceStatus.Ativa;
    }

    public bool EstaAtiva => Status == ApoliceStatus.Ativa;

    public void Ativar() => Status = ApoliceStatus.Ativa;

    public void Inativar() => Status = ApoliceStatus.Inativa;

    public void Cancelar() => Status = ApoliceStatus.Cancelada;
}
