namespace Segfy.Domain.Exceptions;

public sealed class ApoliceInativaException : DomainException
{
    public ApoliceInativaException()
        : base("Não é possível abrir um sinistro para uma apólice que não está ativa.")
    {
    }
}
