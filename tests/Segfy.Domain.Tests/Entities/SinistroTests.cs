using FluentAssertions;
using Segfy.Domain.Entities;
using Segfy.Domain.Enums;
using Segfy.Domain.Exceptions;

namespace Segfy.Domain.Tests.Entities;

public class SinistroTests
{
    private static Apolice CriarApoliceAtiva() => new("AP-0001", Guid.NewGuid(), Guid.NewGuid());

    [Fact]
    public void Abrir_QuandoApoliceAtiva_DeveCriarSinistroComStatusAberto()
    {
        var apolice = CriarApoliceAtiva();

        var sinistro = Sinistro.Abrir(apolice, 1500m);

        sinistro.ApoliceId.Should().Be(apolice.Id);
        sinistro.Status.Should().Be(SinistroStatus.Aberto);
        sinistro.ValorEstimado.Valor.Should().Be(1500m);
        sinistro.ValorAprovado.Should().BeNull();
        sinistro.MotivoNegativa.Should().BeNull();
        sinistro.Historico.Should().BeEmpty();
    }

    [Theory]
    [InlineData(ApoliceStatus.Inativa)]
    [InlineData(ApoliceStatus.Cancelada)]
    public void Abrir_QuandoApoliceNaoEstaAtiva_DeveLancarApoliceInativaException(ApoliceStatus status)
    {
        var apolice = CriarApoliceAtiva();

        if (status == ApoliceStatus.Inativa)
            apolice.Inativar();
        else
            apolice.Cancelar();

        var acao = () => Sinistro.Abrir(apolice, 1000m);

        acao.Should().Throw<ApoliceInativaException>();
    }

    [Fact]
    public void Abrir_QuandoApoliceForNula_DeveLancarArgumentNullException()
    {
        var acao = () => Sinistro.Abrir(null!, 1000m);

        acao.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AlterarStatus_DeAbertoParaEmAnalise_DeveAlterarStatusERegistrarHistorico()
    {
        var sinistro = Sinistro.Abrir(CriarApoliceAtiva(), 1000m);

        sinistro.AlterarStatus(SinistroStatus.EmAnalise);

        sinistro.Status.Should().Be(SinistroStatus.EmAnalise);
        sinistro.Historico.Should().ContainSingle(h =>
            h.StatusAnterior == SinistroStatus.Aberto && h.StatusNovo == SinistroStatus.EmAnalise);
    }

    [Theory]
    [InlineData(SinistroStatus.Aprovado)]
    [InlineData(SinistroStatus.Encerrado)]
    [InlineData(SinistroStatus.Negado)]
    [InlineData(SinistroStatus.Aberto)]
    public void AlterarStatus_APartirDeAberto_ParaStatusNaoPermitido_DeveLancarTransicaoStatusInvalidaException(
        SinistroStatus statusDesejado)
    {
        var sinistro = Sinistro.Abrir(CriarApoliceAtiva(), 1000m);

        var acao = () => sinistro.AlterarStatus(statusDesejado, 100m, "motivo");

        acao.Should().Throw<TransicaoStatusInvalidaException>();
    }

    [Theory]
    [InlineData(SinistroStatus.Aprovado)]
    [InlineData(SinistroStatus.Encerrado)]
    [InlineData(SinistroStatus.Negado)]
    public void AlterarStatus_APartirDeStatusTerminal_DeveLancarTransicaoStatusInvalidaException(
        SinistroStatus statusTerminal)
    {
        var sinistro = Sinistro.Abrir(CriarApoliceAtiva(), 1000m);
        sinistro.AlterarStatus(SinistroStatus.EmAnalise);
        sinistro.AlterarStatus(statusTerminal, 100m, "motivo");

        var acao = () => sinistro.AlterarStatus(SinistroStatus.EmAnalise);

        acao.Should().Throw<TransicaoStatusInvalidaException>();
    }

    [Fact]
    public void AlterarStatus_ParaNegado_SemMotivoNegativa_DeveLancarMotivoNegativaObrigatorioException()
    {
        var sinistro = Sinistro.Abrir(CriarApoliceAtiva(), 1000m);
        sinistro.AlterarStatus(SinistroStatus.EmAnalise);

        var acao = () => sinistro.AlterarStatus(SinistroStatus.Negado);

        acao.Should().Throw<MotivoNegativaObrigatorioException>();
    }

    [Fact]
    public void AlterarStatus_ParaNegado_ComMotivoNegativa_DeveAlterarStatusEDefinirMotivo()
    {
        var sinistro = Sinistro.Abrir(CriarApoliceAtiva(), 1000m);
        sinistro.AlterarStatus(SinistroStatus.EmAnalise);

        sinistro.AlterarStatus(SinistroStatus.Negado, motivoNegativa: "Sinistro fraudulento");

        sinistro.Status.Should().Be(SinistroStatus.Negado);
        sinistro.MotivoNegativa.Should().Be("Sinistro fraudulento");
    }

    [Theory]
    [InlineData(SinistroStatus.Aprovado)]
    [InlineData(SinistroStatus.Encerrado)]
    public void AlterarStatus_ParaAprovadoOuEncerrado_SemValorAprovado_DeveLancarValorAprovadoObrigatorioException(
        SinistroStatus statusDesejado)
    {
        var sinistro = Sinistro.Abrir(CriarApoliceAtiva(), 1000m);
        sinistro.AlterarStatus(SinistroStatus.EmAnalise);

        var acao = () => sinistro.AlterarStatus(statusDesejado);

        acao.Should().Throw<ValorAprovadoObrigatorioException>();
    }

    [Theory]
    [InlineData(SinistroStatus.Aprovado)]
    [InlineData(SinistroStatus.Encerrado)]
    public void AlterarStatus_ParaAprovadoOuEncerrado_ComValorAprovado_DeveDefinirValorAprovado(
        SinistroStatus statusDesejado)
    {
        var sinistro = Sinistro.Abrir(CriarApoliceAtiva(), 1000m);
        sinistro.AlterarStatus(SinistroStatus.EmAnalise);

        sinistro.AlterarStatus(statusDesejado, valorAprovado: 900m);

        sinistro.Status.Should().Be(statusDesejado);
        sinistro.ValorAprovado!.Valor.Should().Be(900m);
    }

    [Fact]
    public void AlterarStatus_DeveRegistrarUmHistoricoParaCadaTransicao()
    {
        var sinistro = Sinistro.Abrir(CriarApoliceAtiva(), 1000m);

        sinistro.AlterarStatus(SinistroStatus.EmAnalise);
        sinistro.AlterarStatus(SinistroStatus.Aprovado, valorAprovado: 800m);

        sinistro.Historico.Should().HaveCount(2);
        sinistro.Historico.Should().Contain(h =>
            h.StatusAnterior == SinistroStatus.Aberto && h.StatusNovo == SinistroStatus.EmAnalise);
        sinistro.Historico.Should().Contain(h =>
            h.StatusAnterior == SinistroStatus.EmAnalise && h.StatusNovo == SinistroStatus.Aprovado);
    }
}
