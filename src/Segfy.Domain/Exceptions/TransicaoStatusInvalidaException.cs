using Segfy.Domain.Enums;

namespace Segfy.Domain.Exceptions;

public sealed class TransicaoStatusInvalidaException : DomainException
{
    public TransicaoStatusInvalidaException(SinistroStatus statusAtual, SinistroStatus statusDesejado)
        : base($"Não é possível alterar o status de '{statusAtual}' para '{statusDesejado}'.")
    {
    }
}
