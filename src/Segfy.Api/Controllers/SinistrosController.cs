using MediatR;
using Microsoft.AspNetCore.Mvc;
using Segfy.Api.Contracts;
using Segfy.Application.Common.Models;
using Segfy.Application.Sinistros.Commands.AbrirSinistro;
using Segfy.Application.Sinistros.Commands.AlterarStatusSinistro;
using Segfy.Application.Sinistros.Dtos;
using Segfy.Application.Sinistros.Queries.ListarSinistros;
using Segfy.Application.Sinistros.Queries.ObterHistoricoSinistro;
using Segfy.Application.Sinistros.Queries.ObterSinistroPorId;
using Segfy.Domain.Enums;

namespace Segfy.Api.Controllers;

[ApiController]
[Route("api/sinistros")]
public class SinistrosController : ControllerBase
{
    private readonly IMediator _mediator;

    public SinistrosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(SinistroDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SinistroDto>> Abrir(
        [FromBody] AbrirSinistroRequest request, CancellationToken cancellationToken)
    {
        var sinistro = await _mediator.Send(
            new AbrirSinistroCommand(request.ApoliceId, request.ValorEstimado), cancellationToken);

        return CreatedAtAction(nameof(ObterPorId), new { id = sinistro.Id }, sinistro);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SinistroDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SinistroDto>> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var sinistro = await _mediator.Send(new ObterSinistroPorIdQuery(id), cancellationToken);
        return Ok(sinistro);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<SinistroDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<SinistroDto>>> Listar(
        [FromQuery] SinistroStatus? status,
        [FromQuery] DateTime? dataInicio,
        [FromQuery] DateTime? dataFim,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanhoPagina = 10,
        CancellationToken cancellationToken = default)
    {
        var resultado = await _mediator.Send(
            new ListarSinistrosQuery(status, dataInicio, dataFim, pagina, tamanhoPagina), cancellationToken);

        return Ok(resultado);
    }

    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(typeof(SinistroDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SinistroDto>> AlterarStatus(
        Guid id, [FromBody] AlterarStatusRequest request, CancellationToken cancellationToken)
    {
        var sinistro = await _mediator.Send(
            new AlterarStatusSinistroCommand(id, request.NovoStatus, request.ValorAprovado, request.MotivoNegativa),
            cancellationToken);

        return Ok(sinistro);
    }

    [HttpGet("{id:guid}/historico")]
    [ProducesResponseType(typeof(IReadOnlyCollection<HistoricoSinistroDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyCollection<HistoricoSinistroDto>>> ObterHistorico(
        Guid id, CancellationToken cancellationToken)
    {
        var historico = await _mediator.Send(new ObterHistoricoSinistroQuery(id), cancellationToken);
        return Ok(historico);
    }
}
