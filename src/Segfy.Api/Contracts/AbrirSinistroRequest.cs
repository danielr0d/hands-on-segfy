namespace Segfy.Api.Contracts;

public sealed record AbrirSinistroRequest(Guid ApoliceId, decimal ValorEstimado);
