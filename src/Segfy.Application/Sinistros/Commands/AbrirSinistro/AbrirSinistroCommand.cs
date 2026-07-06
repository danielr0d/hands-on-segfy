using MediatR;
using Segfy.Application.Sinistros.Dtos;

namespace Segfy.Application.Sinistros.Commands.AbrirSinistro;

public sealed record AbrirSinistroCommand(Guid ApoliceId, decimal ValorEstimado) : IRequest<SinistroDto>;
