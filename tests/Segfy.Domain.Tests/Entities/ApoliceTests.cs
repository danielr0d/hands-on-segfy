using FluentAssertions;
using Segfy.Domain.Entities;
using Segfy.Domain.Enums;
using Segfy.Domain.Exceptions;

namespace Segfy.Domain.Tests.Entities;

public class ApoliceTests
{
    [Fact]
    public void Constructor_ComDadosValidos_DeveCriarApoliceAtiva()
    {
        var clienteId = Guid.NewGuid();
        var ramoId = Guid.NewGuid();

        var apolice = new Apolice("AP-0001", clienteId, ramoId);

        apolice.Numero.Should().Be("AP-0001");
        apolice.ClienteId.Should().Be(clienteId);
        apolice.RamoId.Should().Be(ramoId);
        apolice.Status.Should().Be(ApoliceStatus.Ativa);
        apolice.EstaAtiva.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ComNumeroInvalido_DeveLancarCampoObrigatorioException(string? numero)
    {
        var acao = () => new Apolice(numero!, Guid.NewGuid(), Guid.NewGuid());

        acao.Should().Throw<CampoObrigatorioException>();
    }

    [Fact]
    public void Constructor_ComClienteIdVazio_DeveLancarCampoObrigatorioException()
    {
        var acao = () => new Apolice("AP-0001", Guid.Empty, Guid.NewGuid());

        acao.Should().Throw<CampoObrigatorioException>();
    }

    [Fact]
    public void Constructor_ComRamoIdVazio_DeveLancarCampoObrigatorioException()
    {
        var acao = () => new Apolice("AP-0001", Guid.NewGuid(), Guid.Empty);

        acao.Should().Throw<CampoObrigatorioException>();
    }

    [Fact]
    public void Inativar_DeveAlterarStatusParaInativaENaoEstarMaisAtiva()
    {
        var apolice = new Apolice("AP-0001", Guid.NewGuid(), Guid.NewGuid());

        apolice.Inativar();

        apolice.Status.Should().Be(ApoliceStatus.Inativa);
        apolice.EstaAtiva.Should().BeFalse();
    }

    [Fact]
    public void Cancelar_DeveAlterarStatusParaCanceladaENaoEstarMaisAtiva()
    {
        var apolice = new Apolice("AP-0001", Guid.NewGuid(), Guid.NewGuid());

        apolice.Cancelar();

        apolice.Status.Should().Be(ApoliceStatus.Cancelada);
        apolice.EstaAtiva.Should().BeFalse();
    }

    [Fact]
    public void Ativar_AposInativar_DeveVoltarAEstarAtiva()
    {
        var apolice = new Apolice("AP-0001", Guid.NewGuid(), Guid.NewGuid());
        apolice.Inativar();

        apolice.Ativar();

        apolice.Status.Should().Be(ApoliceStatus.Ativa);
        apolice.EstaAtiva.Should().BeTrue();
    }
}
