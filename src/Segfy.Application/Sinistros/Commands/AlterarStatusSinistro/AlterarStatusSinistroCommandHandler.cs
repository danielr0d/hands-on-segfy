using MediatR;
using Segfy.Application.Common.Exceptions;
using Segfy.Application.Common.Interfaces;
using Segfy.Application.Sinistros.Dtos;
using Segfy.Domain.Entities;

namespace Segfy.Application.Sinistros.Commands.AlterarStatusSinistro;

public sealed class AlterarStatusSinistroCommandHandler : IRequestHandler<AlterarStatusSinistroCommand, SinistroDto>
{
    private readonly ISinistroRepository _sinistroRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AlterarStatusSinistroCommandHandler(ISinistroRepository sinistroRepository, IUnitOfWork unitOfWork)
    {
        _sinistroRepository = sinistroRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SinistroDto> Handle(AlterarStatusSinistroCommand request, CancellationToken cancellationToken)
    {
        var sinistro = await _sinistroRepository.ObterPorIdAsync(request.SinistroId, cancellationToken)
            ?? throw new NotFoundException(nameof(Sinistro), request.SinistroId);

        sinistro.AlterarStatus(request.NovoStatus, request.ValorAprovado, request.MotivoNegativa);

        await _unitOfWork.SalvarAlteracoesAsync(cancellationToken);

        return SinistroDto.FromEntity(sinistro);
    }
}
