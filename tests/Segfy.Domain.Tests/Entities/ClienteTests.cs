using FluentAssertions;
using Segfy.Domain.Entities;
using Segfy.Domain.Exceptions;

namespace Segfy.Domain.Tests.Entities;

public class ClienteTests
{
    [Fact]
    public void Constructor_ComNomeValido_DeveCriarCliente()
    {
        var cliente = new Cliente("Ana Beatriz Souza");

        cliente.Nome.Should().Be("Ana Beatriz Souza");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ComNomeInvalido_DeveLancarCampoObrigatorioException(string? nome)
    {
        var acao = () => new Cliente(nome!);

        acao.Should().Throw<CampoObrigatorioException>();
    }

    [Fact]
    public void AlterarNome_ComNomeValido_DeveAtualizarNome()
    {
        var cliente = new Cliente("Ana Beatriz Souza");

        cliente.AlterarNome("Ana B. Souza");

        cliente.Nome.Should().Be("Ana B. Souza");
    }
}
