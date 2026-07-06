namespace Segfy.Domain.Exceptions;

public sealed class MotivoNegativaObrigatorioException : DomainException
{
    public MotivoNegativaObrigatorioException()
        : base("O motivo da negativa é obrigatório ao negar um sinistro.")
    {
    }
}
