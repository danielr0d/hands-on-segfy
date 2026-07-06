using FluentAssertions;
using Segfy.Domain.Exceptions;
using Segfy.Domain.ValueObjects;

namespace Segfy.Domain.Tests.ValueObjects;

public class DinheiroTests
{
    [Fact]
    public void Constructor_ComValorNegativo_DeveLancarValorMonetarioInvalidoException()
    {
        var acao = () => new Dinheiro(-0.01m);

        acao.Should().Throw<ValorMonetarioInvalidoException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(0.01)]
    [InlineData(1500.75)]
    public void Constructor_ComValorValido_DeveArmazenarValor(decimal valor)
    {
        var dinheiro = new Dinheiro(valor);

        dinheiro.Valor.Should().Be(valor);
    }

    [Fact]
    public void ConversaoImplicitaParaDecimal_DeveRetornarValorArmazenado()
    {
        var dinheiro = new Dinheiro(250m);

        decimal valor = dinheiro;

        valor.Should().Be(250m);
    }

    [Fact]
    public void Equals_ComMesmoValor_DeveSeremIguais()
    {
        var a = new Dinheiro(100m);
        var b = new Dinheiro(100m);

        a.Should().Be(b);
    }
}
