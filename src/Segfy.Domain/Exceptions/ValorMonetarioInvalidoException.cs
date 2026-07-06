namespace Segfy.Domain.Exceptions;

public sealed class ValorMonetarioInvalidoException : DomainException
{
    public ValorMonetarioInvalidoException()
        : base("O valor monetário não pode ser negativo.")
    {
    }
}
