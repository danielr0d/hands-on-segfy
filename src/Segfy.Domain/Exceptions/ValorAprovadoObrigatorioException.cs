namespace Segfy.Domain.Exceptions;

public sealed class ValorAprovadoObrigatorioException : DomainException
{
    public ValorAprovadoObrigatorioException()
        : base("O valor aprovado é obrigatório ao aprovar ou encerrar um sinistro.")
    {
    }
}
