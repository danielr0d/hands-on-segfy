namespace Segfy.Domain.Exceptions;

public sealed class CampoObrigatorioException : DomainException
{
    public CampoObrigatorioException(string nomeCampo)
        : base($"O campo '{nomeCampo}' é obrigatório.")
    {
    }
}
