using FluentAssertions;
using Moq;
using Segfy.Application.Common.Exceptions;
using Segfy.Application.Common.Interfaces;
using Segfy.Application.Sinistros.Queries.ObterSinistroPorId;
using Segfy.Domain.Entities;

namespace Segfy.Application.Tests.Sinistros.Queries;

public class ObterSinistroPorIdQueryHandlerTests
{
    private readonly Mock<ISinistroRepository> _sinistroRepository = new();
    private readonly ObterSinistroPorIdQueryHandler _handler;

    public ObterSinistroPorIdQueryHandlerTests()
    {
        _handler = new ObterSinistroPorIdQueryHandler(_sinistroRepository.Object);
    }

    [Fact]
    public async Task Handle_QuandoSinistroExiste_DeveRetornarDtoCorrespondente()
    {
        var sinistro = Sinistro.Abrir(new Apolice("AP-0001", Guid.NewGuid(), Guid.NewGuid()), 1200m);

        _sinistroRepository
            .Setup(r => r.ObterPorIdAsync(sinistro.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(sinistro);

        var resultado = await _handler.Handle(new ObterSinistroPorIdQuery(sinistro.Id), CancellationToken.None);

        resultado.Id.Should().Be(sinistro.Id);
        resultado.ValorEstimado.Should().Be(1200m);
    }

    [Fact]
    public async Task Handle_QuandoSinistroNaoExiste_DeveLancarNotFoundException()
    {
        var id = Guid.NewGuid();

        _sinistroRepository
            .Setup(r => r.ObterPorIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Sinistro?)null);

        var acao = () => _handler.Handle(new ObterSinistroPorIdQuery(id), CancellationToken.None);

        await acao.Should().ThrowAsync<NotFoundException>();
    }
}
