namespace Segfy.Application.Common.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string entityName, object key)
        : base($"'{entityName}' com identificador '{key}' não foi encontrado(a).")
    {
    }
}
