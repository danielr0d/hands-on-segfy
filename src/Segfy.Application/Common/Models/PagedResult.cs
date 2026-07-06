namespace Segfy.Application.Common.Models;

public sealed class PagedResult<T>
{
    public IReadOnlyCollection<T> Itens { get; }
    public int Pagina { get; }
    public int TamanhoPagina { get; }
    public int TotalRegistros { get; }
    public int TotalPaginas => TamanhoPagina == 0 ? 0 : (int)Math.Ceiling(TotalRegistros / (double)TamanhoPagina);

    public PagedResult(IReadOnlyCollection<T> itens, int pagina, int tamanhoPagina, int totalRegistros)
    {
        Itens = itens;
        Pagina = pagina;
        TamanhoPagina = tamanhoPagina;
        TotalRegistros = totalRegistros;
    }
}
