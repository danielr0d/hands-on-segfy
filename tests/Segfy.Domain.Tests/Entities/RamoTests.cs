using FluentAssertions;
using Segfy.Domain.Entities;
using Segfy.Domain.Exceptions;

namespace Segfy.Domain.Tests.Entities;

public class RamoTests
{
    [Fact]
    public void Constructor_ComNomeValido_DeveCriarRamo()
    {
        var ramo = new Ramo("Automóvel");

        ramo.Nome.Should().Be("Automóvel");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ComNomeInvalido_DeveLancarCampoObrigatorioException(string? nome)
    {
        var acao = () => new Ramo(nome!);

        acao.Should().Throw<CampoObrigatorioException>();
    }
}
