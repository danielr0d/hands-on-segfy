using Segfy.Domain.Exceptions;

namespace Segfy.Domain.ValueObjects;

public sealed record Dinheiro
{
    public decimal Valor { get; }

    public Dinheiro(decimal valor)
    {
        if (valor < 0)
            throw new ValorMonetarioInvalidoException();

        Valor = valor;
    }

    public static implicit operator decimal(Dinheiro dinheiro) => dinheiro.Valor;

    public override string ToString() => Valor.ToString("C");
}
