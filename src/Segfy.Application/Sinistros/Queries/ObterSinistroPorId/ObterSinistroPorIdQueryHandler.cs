using MediatR;
using Segfy.Application.Common.Exceptions;
using Segfy.Application.Common.Interfaces;
using Segfy.Application.Sinistros.Dtos;
using Segfy.Domain.Entities;

namespace Segfy.Application.Sinistros.Queries.ObterSinistroPorId;

public sealed class ObterSinistroPorIdQueryHandler : IRequestHandler<ObterSinistroPorIdQuery, SinistroDto>
{
    private readonly ISinistroRepository _sinistroRepository;

    public ObterSinistroPorIdQueryHandler(ISinistroRepository sinistroRepository)
    {
        _sinistroRepository = sinistroRepository;
    }

    public async Task<SinistroDto> Handle(ObterSinistroPorIdQuery request, CancellationToken cancellationToken)
    {
        var sinistro = await _sinistroRepository.ObterPorIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Sinistro), request.Id);

        return SinistroDto.FromEntity(sinistro);
    }
}
