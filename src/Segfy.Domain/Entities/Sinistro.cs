using Segfy.Domain.Common;
using Segfy.Domain.Enums;
using Segfy.Domain.Exceptions;
using Segfy.Domain.ValueObjects;

namespace Segfy.Domain.Entities;

public class Sinistro : BaseEntity
{
    private static readonly Dictionary<SinistroStatus, SinistroStatus[]> TransicoesPermitidas = new()
    {
        [SinistroStatus.Aberto] = [SinistroStatus.EmAnalise],
        [SinistroStatus.EmAnalise] = [SinistroStatus.Aprovado, SinistroStatus.Encerrado, SinistroStatus.Negado]
    };

    private readonly List<HistoricoSinistro> _historico = [];

    public Guid ApoliceId { get; private set; }
    public DateTime DataAbertura { get; private set; }
    public SinistroStatus Status { get; private set; }
    public Dinheiro ValorEstimado { get; private set; } = null!;
    public Dinheiro? ValorAprovado { get; private set; }
    public string? MotivoNegativa { get; private set; }
    public IReadOnlyCollection<HistoricoSinistro> Historico => _historico.AsReadOnly();

    protected Sinistro()
    {
    }

    private Sinistro(Guid apoliceId, decimal valorEstimado)
    {
        ApoliceId = apoliceId;
        DataAbertura = DateTime.UtcNow;
        Status = SinistroStatus.Aberto;
        ValorEstimado = new Dinheiro(valorEstimado);
    }

    public static Sinistro Abrir(Apolice apolice, decimal valorEstimado)
    {
        ArgumentNullException.ThrowIfNull(apolice);

        if (!apolice.EstaAtiva)
            throw new ApoliceInativaException();

        return new Sinistro(apolice.Id, valorEstimado);
    }

    public void AlterarStatus(SinistroStatus novoStatus, decimal? valorAprovado = null, string? motivoNegativa = null)
    {
        ValidarTransicao(novoStatus);

        if (novoStatus == SinistroStatus.Negado && string.IsNullOrWhiteSpace(motivoNegativa))
            throw new MotivoNegativaObrigatorioException();

        if (novoStatus is SinistroStatus.Aprovado or SinistroStatus.Encerrado && valorAprovado is null)
            throw new ValorAprovadoObrigatorioException();

        _historico.Add(new HistoricoSinistro(Id, Status, novoStatus));

        Status = novoStatus;

        if (valorAprovado is not null)
            ValorAprovado = new Dinheiro(valorAprovado.Value);

        if (motivoNegativa is not null)
            MotivoNegativa = motivoNegativa;
    }

    private void ValidarTransicao(SinistroStatus novoStatus)
    {
        if (!TransicoesPermitidas.TryGetValue(Status, out var permitidos) || !permitidos.Contains(novoStatus))
            throw new TransicaoStatusInvalidaException(Status, novoStatus);
    }
}
