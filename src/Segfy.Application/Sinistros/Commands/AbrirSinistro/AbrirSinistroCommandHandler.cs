using MediatR;
using Segfy.Application.Common.Exceptions;
using Segfy.Application.Common.Interfaces;
using Segfy.Application.Sinistros.Dtos;
using Segfy.Domain.Entities;

namespace Segfy.Application.Sinistros.Commands.AbrirSinistro;

public sealed class AbrirSinistroCommandHandler : IRequestHandler<AbrirSinistroCommand, SinistroDto>
{
    private readonly IApoliceRepository _apoliceRepository;
    private readonly ISinistroRepository _sinistroRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AbrirSinistroCommandHandler(
        IApoliceRepository apoliceRepository,
        ISinistroRepository sinistroRepository,
        IUnitOfWork unitOfWork)
    {
        _apoliceRepository = apoliceRepository;
        _sinistroRepository = sinistroRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SinistroDto> Handle(AbrirSinistroCommand request, CancellationToken cancellationToken)
    {
        var apolice = await _apoliceRepository.ObterPorIdAsync(request.ApoliceId, cancellationToken)
            ?? throw new NotFoundException(nameof(Apolice), request.ApoliceId);

        var sinistro = Sinistro.Abrir(apolice, request.ValorEstimado);

        await _sinistroRepository.AdicionarAsync(sinistro, cancellationToken);
        await _unitOfWork.SalvarAlteracoesAsync(cancellationToken);

        return SinistroDto.FromEntity(sinistro);
    }
}
