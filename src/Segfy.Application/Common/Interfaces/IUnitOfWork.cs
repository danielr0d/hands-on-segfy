namespace Segfy.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken);
}
