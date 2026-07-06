using MediatR;
using Segfy.Application.Common.Interfaces;
using Segfy.Application.Common.Models;
using Segfy.Application.Sinistros.Dtos;

namespace Segfy.Application.Sinistros.Queries.ListarSinistros;

public sealed class ListarSinistrosQueryHandler : IRequestHandler<ListarSinistrosQuery, PagedResult<SinistroDto>>
{
    private readonly ISinistroRepository _sinistroRepository;

    public ListarSinistrosQueryHandler(ISinistroRepository sinistroRepository)
    {
        _sinistroRepository = sinistroRepository;
    }

    public async Task<PagedResult<SinistroDto>> Handle(ListarSinistrosQuery request, CancellationToken cancellationToken)
    {
        var (itens, totalRegistros) = await _sinistroRepository.ListarAsync(
            request.Status,
            request.DataInicio,
            request.DataFim,
            request.Pagina,
            request.TamanhoPagina,
            cancellationToken);

        var dtos = itens.Select(SinistroDto.FromEntity).ToList();

        return new PagedResult<SinistroDto>(dtos, request.Pagina, request.TamanhoPagina, totalRegistros);
    }
}
